using System;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using Neptuo.Templates.VisualStudio.IntelliSense.Completions;

namespace Neptuo.Templates.VisualStudio.IntelliSense
{
    internal class ViewCommandHandler : IOleCommandTarget
    {
        private readonly CompletionContext completionContext;
        private readonly IOleCommandTarget nextController;
        private readonly ITextView textView;
        private readonly SVsServiceProvider serviceProvider;

        public ViewCommandHandler(CompletionContext completionContext, IVsTextView textViewAdapter, ITextView textView, SVsServiceProvider serviceProvider)
        {
            this.completionContext = completionContext;
            this.textView = textView;
            this.serviceProvider = serviceProvider;

            // Add the command to the command chain.
            textViewAdapter.AddCommandFilter(this, out nextController);
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            if (pguidCmdGroup == VSConstants.VSStd2K)
            {
                switch ((VSConstants.VSStd2KCmdID)prgCmds[0].cmdID)
                {
                    case VSConstants.VSStd2KCmdID.AUTOCOMPLETE:
                    case VSConstants.VSStd2KCmdID.SHOWMEMBERLIST:
                    case VSConstants.VSStd2KCmdID.COMPLETEWORD:
                        prgCmds[0].cmdf = (uint)OLECMDF.OLECMDF_ENABLED | (uint)OLECMDF.OLECMDF_SUPPORTED;
                        return VSConstants.S_OK;
                }
            }

            return nextController.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            // If inside UI automation, promote execution to next controller.
            if (VsShellUtilities.IsInAutomationFunction(serviceProvider))
                return nextController.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);

            //make a copy of this so we can look at it after forwarding some commands 
            uint commandID = nCmdID;
            char typedChar = Char.MinValue;
            
            // Try to read input as char.
            if (pguidCmdGroup == VSConstants.VSStd2K && nCmdID == (uint)VSConstants.VSStd2KCmdID.TYPECHAR)
                typedChar = (char)(ushort)Marshal.GetObjectForNativeVariant(pvaIn);

            if (completionContext.HasSession)
            {
                if (nCmdID == (uint)VSConstants.VSStd2KCmdID.RETURN || nCmdID == (uint)VSConstants.VSStd2KCmdID.TAB)
                {
                    completionContext.TryCommit();
                    completionContext.TryDismiss();
                    return VSConstants.S_OK;
                }
            }
            else
            {
                if (nCmdID == (uint)VSConstants.VSStd2KCmdID.COMPLETEWORD)
                {
                    completionContext.TryStartSession();
                    return VSConstants.S_OK;
                }
            }

            // Let input be written into the buffer.
            int nextResult = nextController.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);

            // On text input, filter out completion.
            if (!typedChar.Equals(Char.MinValue))
            {
                if (!completionContext.TryInsertAutomaticContent())
                {
                    if (completionContext.IsStartToken())
                        completionContext.TryStartSession();
                    else if (completionContext.IsCommitToken())
                        completionContext.TryCommit();
                    else if (completionContext.HasSession)
                        completionContext.TryFilter();
                }

                return VSConstants.S_OK;
            }
            
            // Return value from next controller.
            return nextResult;
        }
    }
}
