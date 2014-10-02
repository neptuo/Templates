using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Reflection;
using Neptuo.Templates.Compilation.CodeObjects;

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
        /// <returns>Parsed code object; <c>null</c> otherwise.</returns>
        ICodeObject Parse(string content, IValueParserContext context);
    }
}
