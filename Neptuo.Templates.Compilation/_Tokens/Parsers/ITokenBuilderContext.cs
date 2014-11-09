using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Context of <see cref="ITokenBuilder"/>.
    /// </summary>
    public interface ITokenBuilderContext
    {
        /// <summary>
        /// Current parser context.
        /// </summary>
        IValueParserContext ParserContext { get; }

        /// <summary>
        /// Current parser.
        /// </summary>
        TokenValueParser Parser { get; }
        
        /// <summary>
        /// List of registration of <see cref="ITokenBuilder"/>.
        /// </summary>
        ITokenBuilderFactory BuilderFactory { get; }
    }
}
