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
using Neptuo.Windows.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Test.Templates.VisualStudio.UI.ViewModels;

namespace Test.Templates.VisualStudio.UI.Views
{
    [Guid(MyConstants.ToolWindow.SyntaxTokenWindowString)]
    public class SyntaxTokenWindow : ToolWindowPane
    {
        private readonly CurrentBufferPropertyProvider propertyProvider;
        private readonly DTE dte;
        private ITextView currentTextView;

        private TokenContext currentContext;

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

            dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));

            propertyProvider = new CurrentBufferPropertyProvider()
                .Add<TokenContext>(OnTokenContextChanged);
        }

        private void OnTokenContextChanged(TokenContext context, ITextView textView)
        {
            // Unsubscribe.
            if (currentContext != null)
                currentContext.TokensChanged -= OnCurrentTokensChanged;

            // Subscribe to the new context.
            context.TokensChanged += OnCurrentTokensChanged;
            currentContext = context;
            currentTextView = textView;
            OnCurrentTokensChanged(currentContext);

            Window gotFocus = dte.ActiveWindow;
            if (gotFocus.Caption.EndsWith(NtContentType.FileExtension))
                UpdateTitle(gotFocus.Caption);
        }

        private void UpdateTitle(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                Caption = (L)"Syntax tokens";
            else
                Caption = (L)"Syntax tokens: " + fileName;
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (currentContext != null)
                    currentContext.TokensChanged -= OnCurrentTokensChanged;

                propertyProvider.Dispose();
            }
        }
    }
}
