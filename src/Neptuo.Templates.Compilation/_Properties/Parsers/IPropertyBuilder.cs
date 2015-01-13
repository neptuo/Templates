using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Defines builder properties of type.
    /// </summary>
    public interface IPropertyBuilder
    {
        /// <summary>
        /// Parses <paramref name="value"/> and creates AST for it.
        /// Value is attribute value.
        /// </summary>
        /// <param name="context">Context information.</param>
        /// <param name="value">Source value.</param>
        /// <returns>Parsed property descriptors.</returns>
        IEnumerable<IPropertyDescriptor> TryParse(IPropertyBuilderContext context, ISourceContent value);
    }
}
