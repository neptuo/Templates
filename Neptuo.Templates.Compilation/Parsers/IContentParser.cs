using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Defines content parser.
    /// </summary>
    public interface IContentParser
    {
        /// <summary>
        /// Parses <paramref name="content"/> and generates AST.
        /// </summary>
        /// <param name="content">Template content.</param>
        /// <param name="context">Context information.</param>
        /// <returns>Parsed code object; <c>null</c> otherwise.</returns>
        ICodeObject Parse(string content, IContentParserContext context);
    }
}
