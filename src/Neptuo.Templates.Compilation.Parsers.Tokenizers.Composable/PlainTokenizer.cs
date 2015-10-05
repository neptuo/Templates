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
    public class PlainTokenizer : IComposableTokenizer
    {
        public IList<ComposableToken> Tokenize(ContentDecorator decorator, IComposableTokenizerContext context)
        {
            List<ComposableToken> result = new List<ComposableToken>();
            
            string content = decorator.CurrentToEnd().ToString();
            if (!String.IsNullOrEmpty(content))
            {
                result.Add(new ComposableToken(ComposableTokenType.Text, content)
                {
                    TextSpan = decorator.CurrentContentInfo(),
                    DocumentSpan = decorator.CurrentLineInfo()
                });
            }

            return result;
        }
    }
}
