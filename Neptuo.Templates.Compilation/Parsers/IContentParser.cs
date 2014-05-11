﻿using System;
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
        /// Parses <see cref="content"/> and generates AST.
        /// </summary>
        /// <param name="content">Template content.</param>
        /// <param name="context">Context information.</param>
        /// <returns>True if succeed.</returns>
        bool Parse(string content, IContentParserContext context);
    }
}
