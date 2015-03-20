﻿using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense
{
    internal class CompletionSessionHelper
    {
        private readonly ITextView textView;
        private readonly ICompletionBroker completionBroker;

        private ICompletionSession currentSession;

        public CompletionSessionHelper(ITextView textView, ICompletionBroker completionBroker)
        {
            this.textView = textView;
            this.completionBroker = completionBroker;
        }

        public bool StartSession()
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

        public bool TryCommit()
        {
            //check for a a selection 
            if (HasSession)
            {
                //if the selection is fully selected, commit the current session 
                if (currentSession.SelectedCompletionSet.SelectionStatus.IsSelected)
                {
                    currentSession.Commit();
                    return true;
                }
            }

            return false;
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
    }
}
