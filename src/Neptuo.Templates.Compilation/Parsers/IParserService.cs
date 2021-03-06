﻿using Neptuo.Templates.Compilation.CodeObjects;
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
        /// Returns list of content parsers registered to name <paramref name="name"/>.
        /// Never returns <c>null</c>, always at least empty list to insert new parsers into.
        /// </summary>
        /// <param name="name">Name of parsers.</param>
        /// <returns>List of content parsers registered to name <paramref name="name"/>.</returns>
        IList<IContentParser> GetContentParsers(string name);

        /// <summary>
        /// Returns list of value parsers registered to name <paramref name="name"/>.
        /// Never returns <c>null</c>, always at least empty list to insert new parsers into.
        /// </summary>
        /// <param name="name">Name of parsers.</param>
        /// <returns>List of value parsers registered to name <paramref name="name"/>.</returns>
        IList<IValueParser> GetValueParsers(string name);

        /// <summary>
        /// Parses content using registered content parsers and creates AST.
        /// </summary>
        /// <param name="name">Name of parsers to use.</param>
        /// <param name="content">Template content.</param>
        /// <param name="context">Context information.</param>
        /// <returns>Parsed code object; <c>null</c> otherwise.</returns>
        ICodeObject ProcessContent(string name, ISourceContent content, IParserServiceContext context);

        /// <summary>
        /// Parsers value using registered value parsers and create AST.
        /// </summary>
        /// <param name="name">Name of parsers to use.</param>
        /// <param name="value">Template part content.</param>
        /// <param name="context">Context information.</param>
        /// <returns>Parsed code object; <c>null</c> otherwise.</returns>
        ICodeObject ProcessValue(string name, ISourceContent value, IParserServiceContext context);
    }
}