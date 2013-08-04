using Neptuo.Linq.Expressions;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomDependencyGenerator : ICodeDomDependencyGenerator
    {
        public CodeExpression GenerateCode(CodeDomDependencyContext context, Type type)
        {
            if (type == typeof(IComponentManager))
                return new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), CodeDomStructureGenerator.Names.ComponentManagerField);
            
            if (type == typeof(IDependencyProvider))
                return new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), CodeDomStructureGenerator.Names.DependencyProviderField);
            
            return new CodeCastExpression(
                new CodeTypeReference(type),
                new CodeMethodInvokeExpression(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    CodeDomStructureGenerator.Names.DependencyProviderField
                ),
                TypeHelper.MethodName<IDependencyProvider, Type, string, object>(p => p.Resolve),
                new CodeTypeOfExpression(new CodeTypeReference(type)),
                new CodePrimitiveExpression(null)
            ));
        }
    }
}
