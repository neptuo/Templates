﻿using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
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

        private TokenContext currenContext;

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
            Caption = "Syntax tokens";

            BitmapResourceID = 301;
            BitmapIndex = 1;

            ContentView = new SyntaxTokenView();
            ViewModel = new SyntaxTokenViewModel();

            DTE dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));
            events = dte.Events.WindowEvents;
            events.WindowActivated += OnWindowActivated;

            viewManager = (IVsTextManager)ServiceProvider.GlobalProvider.GetService(typeof(SVsTextManager));
            IComponentModel componentModel = (IComponentModel)ServiceProvider.GlobalProvider.GetService(typeof(SComponentModel));
            adapterService = componentModel.GetService<IVsEditorAdaptersFactoryService>();
        }

        private void OnWindowActivated(Window gotFocus, Window lostFocus)
        {
            TokenContext context;
            if (TryGetTokenContext(out context))
            {
                if (currenContext != context)
                {
                    if (currenContext != null)
                        currenContext.TokensChanged -= OnCurrentTokensChanged;

                    context.TokensChanged += OnCurrentTokensChanged;
                    currenContext = context;
                    OnCurrentTokensChanged(currenContext);
                }
            }
        }

        private void OnCurrentTokensChanged(TokenContext context)
        {
            ViewModel.Tokens.Clear();
            ViewModel.Tokens.AddRange(context.Tokens);
        }

        private bool TryGetTokenContext(out TokenContext context)
        {
            IVsTextView view;
            if (viewManager.GetActiveView(1, null, out view) == 0)
            {
                ITextView textView = adapterService.GetWpfTextView(view);
                ITextBuffer textBuffer = textView.TextBuffer;
                return textBuffer.Properties.TryGetProperty(typeof(TokenContext), out context);
            }

            context = null;
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                events.WindowActivated -= OnWindowActivated;
            }
        }
    }
}
