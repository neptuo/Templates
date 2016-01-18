using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Neptuo;
using Neptuo.Localization;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors;
using Neptuo.Templates.VisualStudio.IntelliSense;
using Neptuo.Windows.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Test.Templates.VisualStudio.IntelliSense;
using Test.Templates.VisualStudio.UI.ViewModels;
using Task = System.Threading.Tasks.Task;
using Thread = System.Threading.Thread;

namespace Test.Templates.VisualStudio.UI.Views
{
    [Guid(MyConstants.ToolWindow.SyntaxNodeWindowString)]
    public class SyntaxNodeWindow : ToolWindowPane
    {
        private readonly ISyntaxNodeVisitor visitor;
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


            visitor = new SyntaxVisitorFactory().Create();
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
            context.Error += OnCurrentErrorRaised;
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

        private Stack<DispatcherHelper> w; 
        private List<DispatcherHelper> workers = new List<DispatcherHelper>();

        private void OnCurrentRootNodeChanged(SyntaxContext context)
        {
            if (context.RootNode != null)
            {
                DispatcherHelper worker = new DispatcherHelper(Dispatcher.CurrentDispatcher);
                worker.Run(() =>
                {
                    if (TryRemoveWorker())
                        DoUpdateView(context);
                }, 2000);
                workers.Add(worker);
            }
        }

        private bool TryRemoveWorker()
        {
            if (workers.Count > 0)
                workers.RemoveAt(0);

            return workers.Count == 0;
        }

        private void DoUpdateView(SyntaxContext context)
        {
            if (context.RootNode != null)
            {
                ViewModelBuilder builder = new ViewModelBuilder();
                visitor.Visit(context.RootNode, builder);

                if (builder.RootViewModel != null)
                {
                    ViewModel = builder.RootViewModel;
                    return;
                }
            }
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


        private class ViewModelBuilder : ISyntaxNodeProcessor
        {
            private readonly List<SyntaxNodeViewModel> parents = new List<SyntaxNodeViewModel>();

            public SyntaxNodeViewModel RootViewModel { get; private set; }

            public bool Process(ISyntaxNode node)
            {
                if (parents.Count == 0)
                {
                    RootViewModel = new SyntaxNodeViewModel(node);
                    parents.Insert(0, RootViewModel);
                    return true;
                }

                if (parents[0].SyntaxNode == node.Parent)
                {
                    SyntaxNodeViewModel viewModel = new SyntaxNodeViewModel(node);
                    parents[0].Children.Add(viewModel);
                    parents.Insert(0, viewModel);
                    return true;
                }
                else 
                {
                    for (int i = 1; i < parents.Count; i++)
                    {
                        if (parents[i].SyntaxNode == node.Parent)
                        {
                            for (int j = 0; j < i; j++)
                                parents.Remove(parents[0]);

                            SyntaxNodeViewModel viewModel = new SyntaxNodeViewModel(node);
                            parents[0].Children.Add(viewModel);
                            parents.Insert(0, viewModel);
                            return true;
                        }
                    }
                }

                //throw Ensure.Exception.NotSupported();
                return true;
            }
        }

    }
}
