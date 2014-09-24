﻿using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Helper for parsing markup extensions.
    /// </summary>
    partial class MarkupExtensionValueParser
    {
        public class Helper
        {
            public IValueParserContext Context { get; protected set; }
            public IMarkupExtensionBuilderRegistry BuilderRegistry { get; protected set; }
            public TokenParser Parser { get; protected set; }

            public Helper(IValueParserContext context, IMarkupExtensionBuilderRegistry builderRegistry)
            {
                Context = context;
                BuilderRegistry = builderRegistry;
                Parser = new TokenParser();
                Parser.Configuration.AllowAttributes = true;
                Parser.Configuration.AllowDefaultAttribute = true;
                Parser.Configuration.AllowMultipleTokens = false;
                Parser.Configuration.AllowTextContent = false;
            }
        }
    }
}