using Neptuo;
using Neptuo.Observables;
using Neptuo.Observables.Collections;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.VisualStudio.UI.ViewModels
{
    public class SyntaxNodeViewModel : ObservableObject
    {
        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                if (errorMessage != value)
                {
                    errorMessage = value;
                    RaisePropertyChanged();
                }
            }
        }

        private INode syntaxNode;
        public INode SyntaxNode
        {
            get { return syntaxNode; }
            set
            {
                if (syntaxNode != value)
                {
                    syntaxNode = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<SyntaxNodeViewModel> Children { get; private set; }

        public SyntaxNodeViewModel()
        {
            Children = new ObservableCollection<SyntaxNodeViewModel>();
        }

        public SyntaxNodeViewModel(string errorMessage)
            : this()
        {
            Ensure.NotNullOrEmpty(errorMessage, "errorMessage");
            ErrorMessage = errorMessage;
        }

        public SyntaxNodeViewModel(INode syntaxNode)
            : this()
        {
            Ensure.NotNull(syntaxNode, "syntaxNode");
            this.syntaxNode = syntaxNode;
        }

        public SyntaxNodeViewModel AddChild(SyntaxNodeViewModel viewModel)
        {
            Ensure.NotNull(viewModel, "viewModel");
            Children.Add(viewModel);
            return this;
        }
    }
}
