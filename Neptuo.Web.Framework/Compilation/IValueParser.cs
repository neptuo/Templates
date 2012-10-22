﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Reflection;

namespace Neptuo.Web.Framework.Compilation
{
    public interface IValueParser
    {
        bool Parse(string content, IValueParserContext context);
    }
}
