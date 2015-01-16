﻿using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Value parser context.
    /// </summary>
    public interface IValueParserContext : IParserServiceContext
    {
        /// <summary>
        /// Name of parsers to use.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Current parser service.
        /// </summary>
        IParserService ParserService { get; }
    }
}
