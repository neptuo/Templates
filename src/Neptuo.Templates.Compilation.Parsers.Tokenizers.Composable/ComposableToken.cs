using Neptuo.ComponentModel;
using Neptuo.ComponentModel.TextOffsets;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    /// <summary>
    /// Token implementation for <see cref="ComposableTokenizer"/>.
    /// </summary>
    public class ComposableToken : IToken<ComposableTokenType>
    {
        public ComposableTokenType Type { get; set; }
        public ILineRangeInfo LineInfo { get; set; }
        public IContentRangeInfo ContentInfo { get; set; }
        public string Text { get; set; }
        public IErrorMessage Error { get; set; }

        public ComposableToken()
        { }

        public ComposableToken(ComposableTokenType type, string text)
        {
            Ensure.NotNull(type, "type");
            Type = type;
            Text = text;
        }

        public override string ToString()
        {
            return String.Format("{0} {1} \"{2}\"", ContentInfo, Type, Text);
        }
    }
}
