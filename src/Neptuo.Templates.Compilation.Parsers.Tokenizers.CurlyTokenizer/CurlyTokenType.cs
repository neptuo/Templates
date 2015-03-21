using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    public enum CurlyTokenType
    {
        Text,

        /// <summary>
        /// '{'
        /// </summary>
        OpenBrace,
        Name,

        /// <summary>
        /// ' '
        /// </summary>
        Whitespace,
        AttributeName,

        /// <summary>
        /// '='
        /// </summary>
        AttributeValueSeparator,
        AttributeValue,

        /// <summary>
        /// ','
        /// </summary>
        AttributeSeparator,

        /// <summary>
        /// '}'
        /// </summary>
        CloseBrace
    }
}
