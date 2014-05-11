using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Defines builder for component.
    /// </summary>
    public interface IComponentBuilder
    {
        /// <summary>
        /// Parses <paramref name="element"/> and creates AST for it.
        /// </summary>
        /// <param name="context">Context information.</param>
        /// <param name="element">Source XML tag to parse.</param>
        void Parse(IContentBuilderContext context, IXmlElement element);
    }
}
