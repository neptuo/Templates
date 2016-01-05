using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Neptuo.Localization;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using Neptuo.Templates.VisualStudio.IntelliSense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Test.Templates.VisualStudio.UI.ViewModels;

namespace Test.Templates.VisualStudio.UI.Views
{
    [Guid("79D6D424-9EFA-4C1C-B0F0-D6F3E65DCE4A")]
    public class SyntaxTokenWindow : ToolWindowPane
    {
        private readonly WindowEvents events;
        private readonly IVsTextManager viewManager;
        private readonly IVsEditorAdaptersFactoryService adapterService;

        private ITextView currentTextView;
        private TokenContext currentContext;
        private string currentCaption;

        public SyntaxTokenView ContentView
        {
            get { return (SyntaxTokenView)Content; }
            set { Content = value; }
        }

        public SyntaxTokenViewModel ViewModel
        {
            get { return (SyntaxTokenViewModel)ContentView.DataContext; }
            set { ContentView.DataContext = value; }
        }

        public SyntaxTokenWindow()
            : base(null)
        {
            UpdateTitle(null);

            BitmapResourceID = 301;
            BitmapIndex = 1;

            // Prepare UI.
            ContentView = new SyntaxTokenView();
            ViewModel = new SyntaxTokenViewModel();
            ViewModel.SelectedTokenChanged += OnSelectedTokenChanged;

            // Subscribe to document events.
            DTE dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));
            events = dte.Events.WindowEvents;
            events.WindowActivated += OnWindowActivated;

            // Find view manager (for determining current view) and WPF view adapter.
            viewManager = (IVsTextManager)ServiceProvider.GlobalProvider.GetService(typeof(SVsTextManager));
            IComponentModel componentModel = (IComponentModel)ServiceProvider.GlobalProvider.GetService(typeof(SComponentModel));
            adapterService = componentModel.GetService<IVsEditorAdaptersFactoryService>();
        }

        private void UpdateTitle(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                Caption = (L)"Syntax tokens";
            else
                Caption = (L)"Syntax tokens: " + fileName;
        }

        private void OnWindowActivated(Window gotFocus, Window lostFocus)
        {
            ITextView textView;
            TokenContext context;

            if (TryGetTokenContext(out context, out textView))
            {
                // If context is different.
                if (currentTextView != textView)
                {
                    // Unsubscribe.
                    if (currentContext != null)
                        currentContext.TokensChanged -= OnCurrentTokensChanged;

                    // Store view for view operations.
                    currentTextView = textView;

                    // Subscribe to the new context.
                    context.TokensChanged += OnCurrentTokensChanged;
                    currentContext = context;
                    OnCurrentTokensChanged(currentContext);

                    UpdateTitle(gotFocus.Caption);
                }
            }
        }

        private void OnSelectedTokenChanged(Token token)
        {
            if (token != null && currentTextView != null)
            {
                // Select token's text.
                if (!token.IsVirtual)
                {
                    SnapshotSpan selection = new SnapshotSpan(currentTextView.TextSnapshot, token.TextSpan.StartIndex, token.TextSpan.Length);
                    currentTextView.Selection.Select(selection, false);
                    currentTextView.Caret.MoveTo(selection.Start);
                }
            }
        }

        private void OnCurrentTokensChanged(TokenContext context)
        {
            // Replace old tokens by the new ones.
            ViewModel.SelectedToken = null;
            ViewModel.Tokens.Clear();
            ViewModel.Tokens.AddRange(context.Tokens);
        }

        /// <summary>
        /// Tries to get <paramref name="context"/> of currently selected document.
        /// </summary>
        /// <param name="context">The token context of current document.</param>
        /// <param name="textView">The current text view.</param>
        /// <returns><c>true</c> if both was found; <c>false</c> otherwise.</returns>
        private bool TryGetTokenContext(out TokenContext context, out ITextView textView)
        {
            IVsTextView view;
            if (viewManager.GetActiveView(1, null, out view) == 0)
            {
                textView = adapterService.GetWpfTextView(view);
                ITextBuffer textBuffer = textView.TextBuffer;
                return textBuffer.Properties.TryGetProperty(typeof(TokenContext), out context);
            }

            textView = null;
            context = null;
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (currentContext != null)
                    currentContext.TokensChanged -= OnCurrentTokensChanged;

                events.WindowActivated -= OnWindowActivated;
            }
        }
    }
}
