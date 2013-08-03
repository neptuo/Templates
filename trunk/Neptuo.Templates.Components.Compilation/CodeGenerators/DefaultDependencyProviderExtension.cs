using Neptuo.Linq.Expressions;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators.Extensions.CodeDom
{
    public class DefaultDependencyProviderExtension : IDependencyProviderExtension
    {
        public CodeExpression GenerateCode(CodeDomGenerator.Context context, Type type)
        {
            if (type == typeof(IComponentManager))
                return new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), CodeDomGenerator.Names.ComponentManagerField);
            
            if (type == typeof(IDependencyProvider))
                return new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), CodeDomGenerator.Names.DependencyProviderField);
            
            if (type == typeof(IViewPage))
                return new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), CodeDomGenerator.Names.ViewPageField);

            return new CodeCastExpression(
                new CodeTypeReference(type),
                new CodeMethodInvokeExpression(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    CodeDomGenerator.Names.DependencyProviderField
                ),
                TypeHelper.MethodName<IDependencyProvider, Type, string, object>(p => p.Resolve),
                new CodeTypeOfExpression(new CodeTypeReference(type)),
                new CodePrimitiveExpression(null)
            ));
        }
    }
}
