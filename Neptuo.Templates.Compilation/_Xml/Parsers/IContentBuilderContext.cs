using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Context for <see cref="IContentBuilder"/>.
    /// </summary>
    public interface IContentBuilderContext
    {
        /// <summary>
        /// Current parser context.
        /// </summary>
        IContentParserContext ParserContext { get; }

        /// <summary>
        /// XML parser.
        /// </summary>
        XmlContentParser Parser { get; }

        /// <summary>
        /// XML parser helper.
        /// </summary>
        XmlContentParser.Helper Helper { get; }

        /// <summary>
        /// Component builder registry.
        /// </summary>
        IContentBuilderRegistry BuilderRegistry { get; }
    }
}