using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using Neptuo.Templates.VisualStudio.IntelliSense.Completions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense
{
    internal class CompletionContext
    {
        private readonly TokenContext tokenContext;
        private readonly ITextView textView;
        private readonly ICompletionBroker completionBroker;
        private readonly IAutomaticCompletionProvider automaticCompletionProvider;

        private readonly List<TokenType> startTokens;
        private readonly List<TokenType> commitTokens;
        private ICompletionSession currentSession;

        public CompletionContext(TokenContext tokenContext, ICompletionTriggerProvider triggerProvider, IAutomaticCompletionProvider automaticCompletionProvider, ITextView textView, ICompletionBroker completionBroker)
        {
            this.tokenContext = tokenContext;
            this.startTokens = triggerProvider.GetStartTriggers().Select(t => t.Type).ToList();
            this.commitTokens = triggerProvider.GetCommitTriggers().Select(t => t.Type).ToList();
            this.automaticCompletionProvider = automaticCompletionProvider;
            this.textView = textView;
            this.completionBroker = completionBroker;
        }

        private Token FindCurrentToken()
        {
            int index;
            return FindCurrentToken(out index);
        }

        private Token FindCurrentToken(out int index)
        {
            SnapshotPoint cursorPosition = textView.Caret.Position.BufferPosition;

            IReadOnlyList<Token> tokens = tokenContext.Tokens;
            for (int i = 0; i < tokens.Count; i++)
            {
                Token token = tokens[i];
                if(token.TextSpan.StartIndex <= cursorPosition && token.TextSpan.StartIndex + token.TextSpan.Length >= cursorPosition)
                {
                    index = i;
                    return token;
                }
            }

            index = -1;
            return null;
        }

        public bool IsStartToken()
        {
            Token currentToken = FindCurrentToken();
            if (currentToken == null)
                return false;

            return startTokens.Contains(currentToken.Type);
        }

        public bool IsCommitToken()
        {
            Token currentToken = FindCurrentToken();
            if (currentToken == null)
                return false;

            return commitTokens.Contains(currentToken.Type);
        }

        public bool TryStartSession()
        {
            if (HasSession)
                return false;

            SnapshotPoint caret = textView.Caret.Position.BufferPosition;
            ITextSnapshot snapshot = caret.Snapshot;

            if (!completionBroker.IsCompletionActive(textView))
                currentSession = completionBroker.CreateCompletionSession(textView, snapshot.CreateTrackingPoint(caret, PointTrackingMode.Positive), true);
            else
                currentSession = completionBroker.GetSessions(textView)[0];

            currentSession.Properties.AddProperty(typeof(Token), FindCurrentToken());
            currentSession.Dismissed += OnSessionDismissed;

            if (!currentSession.IsStarted)
                currentSession.Start();

            return true;
        }

        private void OnSessionDismissed(object sender, EventArgs e)
        {
            currentSession.Dismissed -= this.OnSessionDismissed;
            currentSession = null;
        }

        public CommitResult TryCommit()
        {
            //check for a a selection 
            if (HasSession)
            {
                //if the selection is fully selected, commit the current session 
                if (currentSession.SelectedCompletionSet.SelectionStatus.IsSelected || currentSession.SelectedCompletionSet.SelectionStatus.IsUnique)
                {
                    if (currentSession.SelectedCompletionSet.Moniker == CompletionSource.Moniker)
                    {
                        currentSession.Commit();
                        return CommitResult.Commited;
                    }

                    return CommitResult.OtherMoniker;
                }
            }

            return CommitResult.NoSession;
        }

        public bool TryDismissSession()
        {
            //check for a a selection 
            if (HasSession)
            {
                currentSession.Dismiss();
                return true;
            }

            return false;
        }

        public bool TryFilterSession()
        {
            //check for a a selection 
            if (HasSession)
            {
                Token startedOnToken = currentSession.Properties.GetProperty<Token>(typeof(Token));
                Token currentToken = FindCurrentToken();

                if (currentToken != null && startedOnToken != null)
                {
                    if (startedOnToken.Type == currentToken.Type)
                    {
                        currentSession.Filter();
                    }
                    else
                    {
                        TryDismissSession();
                        TryStartSession();
                    }

                    return true;
                }
            }

            return false;
        }

        public bool HasSession
        {
            get { return currentSession != null && !currentSession.IsDismissed; }
        }

        public bool TryInsertAutomaticContent()
        {
            int index;
            Token currentToken = FindCurrentToken(out index);
            if (currentToken == null)
                return false;

            IAutomaticCompletion completion;
            if (automaticCompletionProvider.TryGet(new TokenCursor(tokenContext.Tokens, index), RelativePosition.Start(), out completion))
            {
                int position = currentToken.TextSpan.StartIndex + currentToken.TextSpan.Length + completion.InsertPosition.Value;
                using (ITextEdit textEdit = textView.TextBuffer.CreateEdit())
                {
                    textEdit.Insert(position, completion.Text);
                    textEdit.Apply();
                }

                textView.Caret.MoveTo(new SnapshotPoint(textView.TextSnapshot, position + completion.Text.Length + completion.CursorPosition.Value));
                if (!TryStartSession())
                    TryFilterSession();
            }

            return false;
        }


        public enum CommitResult
        {
            Commited,
            NoSession,
            OtherMoniker
        }
    }
}
