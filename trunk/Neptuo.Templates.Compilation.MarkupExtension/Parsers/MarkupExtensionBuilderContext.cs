﻿using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class MarkupExtensionBuilderContext : IMarkupExtensionBuilderContext
    {
        public IValueParserContext ParserContext { get; private set; }
        public IPropertyDescriptor Parent { get; private set; }
        public ExtensionValueParser Parser { get; private set; }
        public ExtensionValueParser.Helper Helper { get; private set; }
        public IMarkupExtensionBuilderRegistry BuilderRegistry { get; private set; }

        public MarkupExtensionBuilderContext(ExtensionValueParser parser, ExtensionValueParser.Helper helper)
        {
            Parser = parser;
            Helper = helper;
            ParserContext = helper.Context;
            BuilderRegistry = helper.BuilderRegistry;
        }
    }
}