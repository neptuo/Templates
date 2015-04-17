using Neptuo.ComponentModel;
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
        public ISourceRangeLineInfo LineInfo { get; set; }
        public ISourceContentInfo ContentInfo { get; set; }
        public string Text { get; set; }
        public IErrorMessage Error { get; set; }

        /// <summary>
        /// Whether this token is created without content in the source.
        /// </summary>
        public bool IsVirtual { get; set; }

        /// <summary>
        /// Whether this token contains errors.
        /// </summary>
        public bool HasError
        {
            get { return Error != null; }
        }

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
            string position = null;
            if(IsVirtual)
                position = "<Virtual>";
            else if(ContentInfo != null)
                position = ContentInfo.ToString();
            else if(LineInfo != null)
                position = LineInfo.ToString();

            return String.Format("{0} {1} \"{2}\"{3}", position, Type, Text, HasError ? " !" : "");
        }
    }
}
