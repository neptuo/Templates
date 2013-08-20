﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    public interface IComponentBuilder
    {
        void Parse(IContentBuilderContext context, IXmlElement element);
    }
}