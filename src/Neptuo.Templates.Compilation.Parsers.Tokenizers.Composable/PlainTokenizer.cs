using Neptuo.ComponentModel.TextOffsets;
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
        public IList<ComposableToken> Tokenize(IContentReader reader, IComposableTokenizerContext context)
        {
            List<ComposableToken> result = new List<ComposableToken>();

            int startPosition = Math.Max(reader.Position, 0);
            string content = reader.ReadToEnd().ToString();
            if (!String.IsNullOrEmpty(content))
            {
                result.Add(new ComposableToken(ComposableTokenType.Text, content)
                {
                    ContentInfo = new DefaultContentRangeInfo(startPosition, content.Length)
                });
            }

            return result;
        }
    }
}
