using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Compilation.Parsers.Extensions;
using Neptuo.Web.Framework.Utils;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions.CodeDom
{
    public class DependencyCodeObjectExtension : BaseCodeObjectExtension<DependencyCodeObject>
    {
        protected override CodeExpression GenerateCode(CodeObjectExtensionContext context, DependencyCodeObject codeObject, IPropertyDescriptor propertyDescriptor)
        {
            return new CodeCastExpression(
                new CodeTypeReference(codeObject.TargetType),
                new CodeMethodInvokeExpression(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    CodeDomGenerator.Names.DependencyProviderField
                ),
                TypeHelper.MethodName<IDependencyProvider, Type, string, object>(p => p.Resolve),
                new CodeTypeOfExpression(new CodeTypeReference(codeObject.TargetType)),
                new CodePrimitiveExpression(null)
            ));
        }
    }
}
