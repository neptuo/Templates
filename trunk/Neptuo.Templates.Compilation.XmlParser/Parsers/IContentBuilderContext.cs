﻿using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    public interface IContentBuilderContext
    {
        IContentParserContext ParserContext { get; }
        IPropertyDescriptor Parent { get; }
        XmlContentParser Parser { get; }
        XmlContentParser.Helper Helper { get; }
        IContentBuilderRegistry BuilderRegistry { get; }
    }
}