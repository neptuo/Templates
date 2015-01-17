using Neptuo.Linq.Expressions;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomDependencyGenerator : XICodeDomDependencyGenerator
    {
        public CodeExpression GenerateCode(CodeDomDependencyContext context, Type type)
        {
            if (type == typeof(IDependencyProvider))
                return new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), CodeDomStructureGenerator.Names.DependencyProviderField);
            
            return new CodeMethodInvokeExpression(
                new CodeMethodReferenceExpression(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        CodeDomStructureGenerator.Names.DependencyProviderField
                    ),
                    TypeHelper.MethodName<IDependencyProvider, Type, object>(p => p.Resolve),
                    new CodeTypeReference(type)
                )
            );
        }
    }
}
