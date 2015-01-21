﻿using Neptuo;
using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeGenerators;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.Compilation.CodeGenerators
{
    public class CodeDomDependencyProviderGenerator : ICodeDomDependencyGenerator
    {
        public CodeExpression Generate(ICodeDomContext context, Type type)
        {
            return new CodeMethodInvokeExpression(
                new CodeMethodReferenceExpression(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        "dependencyProvider"
                    ),
                    TypeHelper.MethodName<IDependencyProvider, Type, string, object>(p => p.Resolve),
                    new CodeTypeReference(type)
                )
            );
        }
    }
}