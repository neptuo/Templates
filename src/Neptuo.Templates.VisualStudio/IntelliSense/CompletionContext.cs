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

        private readonly List<TokenType> startTokens;
        private readonly List<TokenType> commitTokens;
        private ICompletionSession currentSession;

        public CompletionContext(TokenContext tokenContext, ITokenTriggerProvider triggerProvider, ITextView textView, ICompletionBroker completionBroker)
        {
            this.tokenContext = tokenContext;
            this.startTokens = triggerProvider.GetStartTriggers().Select(t => t.Type).ToList();
            this.commitTokens = triggerProvider.GetCommitTriggers().Select(t => t.Type).ToList();
            this.textView = textView;
            this.completionBroker = completionBroker;
        }

        private Token FindCurrentToken()
        {
            IReadOnlyList<Token> tokens = tokenContext.Tokens;

            SnapshotPoint cursorPosition = textView.Caret.Position.BufferPosition;
            Token currentToken = tokens.FirstOrDefault(t => t.TextSpan.StartIndex <= cursorPosition && t.TextSpan.StartIndex + t.TextSpan.Length >= cursorPosition);
            return currentToken;
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

        public bool TryDismiss()
        {
            //check for a a selection 
            if (HasSession)
            {
                currentSession.Dismiss();
                return true;
            }

            return false;
        }

        public bool TryFilter()
        {
            //check for a a selection 
            if (HasSession)
            {
                currentSession.Filter();
                return true;
            }

            return false;
        }

        public bool HasSession
        {
            get { return currentSession != null && !currentSession.IsDismissed; }
        }



        public enum CommitResult
        {
            Commited,
            NoSession,
            OtherMoniker
        }
    }
}
