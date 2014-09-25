using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomDependencyObjectGenerator : CodeDomObjectGeneratorBase<DependencyCodeObject>
    {
        protected override CodeExpression GenerateCode(CodeObjectExtensionContext context, DependencyCodeObject codeObject, IPropertyDescriptor propertyDescriptor)
        {
            return new CodeCastExpression(
                new CodeTypeReference(codeObject.TargetType),
                new CodeMethodInvokeExpression(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    CodeDomStructureGenerator.Names.DependencyProviderField
                ),
                TypeHelper.MethodName<IDependencyProvider, Type, string, object>(p => p.Resolve),
                new CodeTypeOfExpression(new CodeTypeReference(codeObject.TargetType)),
                new CodePrimitiveExpression(null)
            ));
        }
    }
}
