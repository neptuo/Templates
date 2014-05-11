﻿using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// CodeDom sub generator.
    /// </summary>
    public interface ICodeDomComponentGenerator
    {
        CodeExpression GenerateCode(CodeObjectExtensionContext context, ICodeObject codeObject, IPropertyDescriptor propertyDescriptor);
    }
}
