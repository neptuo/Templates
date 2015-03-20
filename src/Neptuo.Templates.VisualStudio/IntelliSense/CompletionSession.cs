using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Neptuo.Templates.VisualStudio.IntelliSense.Completions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense
{
    internal class CompletionSession
    {
        private readonly ITextView textView;
        private readonly ICompletionBroker completionBroker;

        private ICompletionSession currentSession;

        public CompletionSession(ITextView textView, ICompletionBroker completionBroker)
        {
            this.textView = textView;
            this.completionBroker = completionBroker;
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
                    if (currentSession.SelectedCompletionSet.Moniker == TemplateCompletionSource.Moniker)
                    {
                        string currentToken = textView.TextBuffer.CurrentSnapshot.GetText().Split(' ').LastOrDefault();
                        if (!String.IsNullOrEmpty(currentToken))
                        {
                            //textView.TextBuffer.Delete(new Span(
                            //    textView.Caret.Position.BufferPosition.Position - currentToken.Length,
                            //    currentToken.Length
                            //));
                            //textView.Caret.MoveTo(new SnapshotPoint(
                            //    textView.Caret.Position.BufferPosition.Snapshot, 
                            //    textView.Caret.Position.BufferPosition.Position - currentToken.Length
                            //));
                        }

                        currentSession.Commit();
                        textView.TextBuffer.Insert(textView.Caret.Position.BufferPosition.Position, "();");
                        textView.Caret.MoveToNextCaretPosition();
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
