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
        /// Registers <paramref name="contentParser"/> with <paramref name="name"/>.
        /// This parser will be inserted to the index 0.
        /// </summary>
        /// <param name="name">Name of parser.</param>
        /// <param name="contentParser">Content parser.</param>
        IParserService AddContentParser(string name, IContentParser contentParser);

        /// <summary>
        /// Registers <paramref name="valueParser"/> with <paramref name="name"/>.
        /// This parser will be inserted to the index 0.
        /// </summary>
        /// <param name="name">Name of parser.</param>
        /// <param name="valueParser">Value parser.</param>
        IParserService AddValueParser(string name, IValueParser valueParser);

        /// <summary>
        /// Parses content using <see cref="ContentParsers"/> and creates AST.
        /// </summary>
        /// <param name="name">Name of parsers to use.</param>
        /// <param name="content">Template content.</param>
        /// <param name="context">Context information.</param>
        /// <returns>Parsed code object; <c>null</c> otherwise.</returns>
        ICodeObject ProcessContent(string name, ISourceContent content, IParserServiceContext context);

        /// <summary>
        /// Parsers value using <see cref="ValueParsers"/> and create AST.
        /// </summary>
        /// <param name="name">Name of parsers to use.</param>
        /// <param name="value">Template part content.</param>
        /// <param name="context">Context information.</param>
        /// <returns>Parsed code object; <c>null</c> otherwise.</returns>
        ICodeObject ProcessValue(string name, ISourceContent value, IParserServiceContext context);
    }
}