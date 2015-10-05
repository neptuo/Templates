using Neptuo.Templates.Compilation.Parsers.Tokenizers.ComponentModel;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    /// <summary>
    /// Parses content as single text token.
    /// </summary>
    public class PlainTokenizer : TokenizerBase
    {
        protected override void Tokenize(ContentDecorator decorator, IComposableTokenizerContext context, List<Token> result)
        {
            string content = decorator.CurrentToEnd().ToString();
            if (!String.IsNullOrEmpty(content))
            {
                result.Add(new Token(TokenType.Text, content)
                {
                    TextSpan = decorator.CurrentContentInfo(),
                    DocumentSpan = decorator.CurrentLineInfo()
                });
            }
        }
    }
}
