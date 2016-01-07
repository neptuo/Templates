﻿using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Neptuo.Localization;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
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
    [Guid("B1B040F1-904C-4AAE-919C-616DB4BFA087")]
    public class SyntaxNodeWindow : ToolWindowPane
    {
        private readonly CurrentBufferPropertyProvider propertyProvider;
        private readonly DTE dte;
        private ITextView currentTextView;

        private SyntaxContext currentContext;

        public SyntaxNodeView ContentView
        {
            get { return (SyntaxNodeView)Content; }
            set { Content = value; }
        }

        public SyntaxNodeViewModel ViewModel
        {
            get { return (SyntaxNodeViewModel)ContentView.DataContext; }
            set { ContentView.DataContext = value; }
        }

        public SyntaxNodeWindow()
            : base(null)
        {
            UpdateTitle(null);

            BitmapResourceID = 301;
            BitmapIndex = 1;

            // Prepare UI.
            ContentView = new SyntaxNodeView();
            ViewModel = new SyntaxNodeViewModel();

            dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));

            propertyProvider = new CurrentBufferPropertyProvider()
                .Add<SyntaxContext>(OnTokenContextChanged);
        }

        private void OnTokenContextChanged(SyntaxContext context, ITextView textView)
        {
            // Unsubscribe.
            if (currentContext != null)
            {
                currentContext.RootNodeChanged -= OnCurrentRootNodeChanged;
                currentContext.Error -= OnCurrentErrorRaised;
            }

            // Subscribe to the new context.
            context.RootNodeChanged += OnCurrentRootNodeChanged;
            currentContext = context;
            currentTextView = textView;
            OnCurrentRootNodeChanged(currentContext);

            Window gotFocus = dte.ActiveWindow;
            if (gotFocus.Caption.EndsWith(NtContentType.FileExtension))
                UpdateTitle(gotFocus.Caption);
        }

        private void UpdateTitle(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                Caption = (L)"Syntax nodes";
            else
                Caption = (L)"Syntax nodes: " + fileName;
        }

        //private void OnSelectedTokenChanged(Token token)
        //{
        //    if (token != null && currentTextView != null)
        //    {
        //        // Select token's text.
        //        if (!token.IsVirtual)
        //        {
        //            SnapshotSpan selection = new SnapshotSpan(currentTextView.TextSnapshot, token.TextSpan.StartIndex, token.TextSpan.Length);
        //            currentTextView.Selection.Select(selection, false);
        //            currentTextView.Caret.MoveTo(selection.Start);
        //        }
        //    }
        //}

        private void OnCurrentRootNodeChanged(SyntaxContext context)
        {
            // TODO: Replace old nodes by the new nodes.
        }

        private void OnCurrentErrorRaised(SyntaxContext context, SyntaxNodeException e)
        {
            // TODO: Show information about error.
            ViewModel.ErrorMessage = e.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (currentContext != null)
                {
                    currentContext.RootNodeChanged -= OnCurrentRootNodeChanged;
                    currentContext.Error -= OnCurrentErrorRaised;
                }

                propertyProvider.Dispose();
            }
        }
    }
}
