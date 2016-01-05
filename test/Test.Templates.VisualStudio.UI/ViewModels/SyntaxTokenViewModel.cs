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
        public ObservableCollection<Token> Tokens { get; private set; }

        public SyntaxTokenViewModel()
        {
            Tokens = new ObservableCollection<Token>();
        }
    }
}
