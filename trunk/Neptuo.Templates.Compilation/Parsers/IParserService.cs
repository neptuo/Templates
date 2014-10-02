using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Parser service.
    /// </summary>
    public interface IParserService
    {
        /// <summary>
        /// List of registered content parsers.
        /// </summary>
        IList<IContentParser> ContentParsers { get; }

        /// <summary>
        /// List of registered value parsers.
        /// </summary>
        IList<IValueParser> ValueParsers { get; }

        /// <summary>
        /// Default value parser if non of <see cref="ValueParsers"/> succeed.
        /// </summary>
        IValueParser DefaultValueParser { get; set; }

        /// <summary>
        /// Parses content using <see cref="ContentParsers"/> and creates AST.
        /// </summary>
        /// <param name="content">Template content.</param>
        /// <param name="context">Context information.</param>
        /// <returns>Parsed code object; <c>null</c> otherwise.</returns>
        ICodeObject ProcessContent(string content, IParserServiceContext context);

        /// <summary>
        /// Parsers value using <see cref="ValueParsers"/> and create AST.
        /// </summary>
        /// <param name="value">Template part content.</param>
        /// <param name="context">Context information.</param>
        /// <returns>Parsed code object; <c>null</c> otherwise.</returns>
        ICodeObject ProcessValue(string value, IParserServiceContext context);
    }
}