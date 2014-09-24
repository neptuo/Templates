﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public interface ICodeDomAttributeGenerator
    {
        CodeExpression GenerateCode(CodeDomAttributeContext context, Attribute attribute);
    }
}