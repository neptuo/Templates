using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers.XmlDocuments
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
        /// Custom values (transient) storage.
        /// </summary>
        Dictionary<string, object> CustomValues { get; }

        /// <summary>
        /// XML parser.
        /// </summary>
        XmlContentParser Parser { get; }

        /// <summary>
        /// Extensible registry for parsers.
        /// </summary>
        IParserRegistry Registry { get; }
    }
}