using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using Neptuo.Text.Positions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Templates.VisualStudio.UI.ViewModels;

namespace Test.Templates.VisualStudio.UI.Views.DesignData
{
    internal class ViewModelLocator
    {
        private static SyntaxTokenViewModel syntaxTokenViewModel;

        public static SyntaxTokenViewModel SyntaxTokenViewModel
        {
            get
            {
                if (syntaxTokenViewModel == null)
                {
                    syntaxTokenViewModel = new SyntaxTokenViewModel();
                    syntaxTokenViewModel.Tokens.Add(new Token(CurlyTokenType.OpenBrace, "{")
                    {
                        TextSpan = new DefaultTextSpan(0, 1),
                        DocumentSpan = new DefaultDocumentSpan(0, 0, 0, 1)
                    });
                    syntaxTokenViewModel.Tokens.Add(new Token(CurlyTokenType.OpenBrace, "Binding")
                    {
                        TextSpan = new DefaultTextSpan(1, 7),
                        DocumentSpan = new DefaultDocumentSpan(0, 1, 0, 8)
                    });
                    syntaxTokenViewModel.Tokens.Add(new Token(CurlyTokenType.OpenBrace, "}")
                    {
                        TextSpan = new DefaultTextSpan(8, 1),
                        DocumentSpan = new DefaultDocumentSpan(0, 8, 0, 9)
                    });
                }

                return syntaxTokenViewModel;
            }
        }


        private static SyntaxNodeViewModel syntaxNodeViewModel;

        public static SyntaxNodeViewModel SyntaxNodeViewModel
        {
            get
            {
                if (syntaxNodeViewModel == null)
                {
                    syntaxNodeViewModel = new SyntaxNodeViewModel();
                }

                return syntaxNodeViewModel;
            }
        }
    }
}
