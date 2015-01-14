using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Context for <see cref="IContentPropertyBuilder"/>.
    /// </summary>
    public interface IContentPropertyBuilderContext : IPropertyBuilderContext
    {
        /// <summary>
        /// Current parser context.
        /// </summary>
        IContentBuilderContext BuilderContext { get; }

        /// <summary>
        /// Custom values (transient) storage.
        /// </summary>
        Dictionary<string, object> CustomValues { get; }

        /// <summary>
        /// XML parser.
        /// </summary>
        XmlContentParser Parser { get; }
    }
}