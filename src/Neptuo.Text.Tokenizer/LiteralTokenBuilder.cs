using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers.ComponentModel;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers
{
    /// <summary>
    /// Parses content as single text token.
    /// </summary>
    public class LiteralTokenBuilder : TokenBuilderBase
    {
        protected override void Tokenize(ContentDecorator decorator, ITokenBuilderContext context, List<Token> result)
        {
            string content = decorator.CurrentToEnd().ToString();
            if (!String.IsNullOrEmpty(content))
            {
                result.Add(new Token(TokenType.Literal, content)
                {
                    TextSpan = decorator.CurrentContentInfo(),
                    DocumentSpan = decorator.CurrentLineInfo()
                });
            }
        }
    }
}
