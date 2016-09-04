using Neptuo.Text.ComponentModel;
using Neptuo.Text.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Text
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
