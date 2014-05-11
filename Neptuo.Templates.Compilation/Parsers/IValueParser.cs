using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Reflection;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Defines value parser.
    /// </summary>
    public interface IValueParser
    {
        /// <summary>
        /// Parses value in <paramref name="content"/> and create AST.
        /// </summary>
        /// <param name="content">Template content part.</param>
        /// <param name="context">Context information.</param>
        /// <returns>True if succeed.</returns>
        bool Parse(string content, IValueParserContext context);
    }
}
