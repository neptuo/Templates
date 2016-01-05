using Neptuo;
using Neptuo.Observables;
using Neptuo.Observables.Collections;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using Neptuo.Templates.VisualStudio.IntelliSense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.VisualStudio.UI.ViewModels
{
    public class SyntaxTokenViewModel : ObservableObject
    {
        private Token selectedToken;
        public Token SelectedToken
        {
            get { return selectedToken; }
            set
            {
                if (selectedToken != value)
                {
                    selectedToken = value;
                    RaisePropertyChanged();

                    if (SelectedTokenChanged != null)
                        SelectedTokenChanged(selectedToken);
                }
            }
        }

        public ObservableCollection<Token> Tokens { get; private set; }

        public event Action<Token> SelectedTokenChanged;

        public SyntaxTokenViewModel()
        {
            Tokens = new ObservableCollection<Token>();
        }
    }
}
