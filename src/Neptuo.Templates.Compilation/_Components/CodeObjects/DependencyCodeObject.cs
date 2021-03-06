﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    public class DependencyCodeObject : ICodeObject
    {
        public Type TargetType { get; set; }

        public DependencyCodeObject(Type targetType)
        {
            TargetType = targetType;
        }
    }
}
