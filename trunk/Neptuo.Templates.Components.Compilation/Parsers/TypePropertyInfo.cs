﻿using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class TypePropertyInfo : IPropertyInfo
    {
        public PropertyInfo PropertyInfo { get; private set; }

        public string Name
        {
            get { return PropertyInfo.Name; }
        }

        public Type Type
        {
            get { return PropertyInfo.PropertyType; }
        }

        public TypePropertyInfo(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }
    }
}
