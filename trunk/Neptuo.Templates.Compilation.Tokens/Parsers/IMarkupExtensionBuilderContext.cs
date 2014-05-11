using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Context of <see cref="IMarkupExtensionBuilder"/>.
    /// </summary>
    public interface IMarkupExtensionBuilderContext
    {
        /// <summary>
        /// Current parser context.
        /// </summary>
        IValueParserContext ParserContext { get; }

        /// <summary>
        /// AST property where this extension will be set.
        /// </summary>
        IPropertyDescriptor Parent { get; }

        /// <summary>
        /// Current parser.
        /// </summary>
        MarkupExtensionValueParser Parser { get; }
        
        /// <summary>
        /// Current parser helper.
        /// </summary>
        MarkupExtensionValueParser.Helper Helper { get; }

        /// <summary>
        /// List of registration of <see cref="IMarkupExtensionBuilder"/>.
        /// </summary>
        IMarkupExtensionBuilderRegistry BuilderRegistry { get; }
    }
}
