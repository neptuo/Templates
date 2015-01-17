﻿using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public abstract class CodeDomObjectGeneratorBase<T> : ICodeDomObjectGenerator
        where T : ICodeObject
    {
        public ICodeDomObjectResult Generate(ICodeDomContext context, ICodeObject codeObject)
        {
            return Generate(context, (T)codeObject);
        }

        protected abstract ICodeDomObjectResult Generate(ICodeDomContext context, T codeObject);
    }
}
