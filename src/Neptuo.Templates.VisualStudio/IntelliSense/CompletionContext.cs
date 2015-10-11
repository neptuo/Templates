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

        private readonly List<TokenType> completableTokens = new List<TokenType>() { CurlyTokenType.OpenBrace, CurlyTokenType.Name, CurlyTokenType.NamePrefix, CurlyTokenType.AttributeName, TokenType.Whitespace };
        private ICompletionSession currentSession;

        public CompletionContext(TokenContext tokenContext, ITextView textView, ICompletionBroker completionBroker)
        {
            this.tokenContext = tokenContext;
            this.textView = textView;
            this.completionBroker = completionBroker;
        }

        public bool IsCompletableToken()
        {
            IList<Token> tokens = tokenContext.Tokens;

            SnapshotPoint cursorPosition = textView.Caret.Position.BufferPosition;
            Token currentToken = tokens.FirstOrDefault(t => t.TextSpan.StartIndex <= cursorPosition && t.TextSpan.StartIndex + t.TextSpan.Length >= cursorPosition);

            return completableTokens.Contains(currentToken.Type);
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
                if (currentSession.SelectedCompletionSet.SelectionStatus.IsSelected)
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
