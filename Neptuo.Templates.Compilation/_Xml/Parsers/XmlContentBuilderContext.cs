﻿using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Base implementation of <see cref="IContentBuilderContext"/>.
    /// </summary>
    public class XmlContentBuilderContext : IContentBuilderContext
    {
        public IContentParserContext ParserContext { get; private set; }
        public XmlContentParser Parser { get; private set; }

        public XmlContentBuilderContext(IContentParserContext parserContext, XmlContentParser parser)
        {
            Guard.NotNull(parserContext, "parserContext");
            Guard.NotNull(parser, "parser");
            ParserContext = parserContext;
            Parser = parser;
        }
    }
}
