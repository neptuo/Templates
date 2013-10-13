﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public interface ICodeDomPropertyTypeGenerator
    {
        CodeExpression GenerateCode(CodeDomPropertyTypeGeneratorContext context, PropertyInfo propertyInfo);
    }
}
