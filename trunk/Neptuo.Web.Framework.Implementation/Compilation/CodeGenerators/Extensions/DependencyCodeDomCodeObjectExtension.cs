using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Compilation.Parsers.Extensions;
using Neptuo.Web.Framework.Utils;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions
{
    public class DependencyCodeDomCodeObjectExtension : BaseCodeDomCodeObjectExtension<DependencyCodeObject>
    {
        protected override CodeExpression GenerateCode(CodeDomCodeObjectExtensionContext context, DependencyCodeObject codeObject, IPropertyDescriptor propertyDescriptor)
        {
            return new CodeCastExpression(
                new CodeTypeReference(codeObject.TargetType),
                new CodeMethodInvokeExpression(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    CodeDomGenerator.Names.ServiceProviderField
                ),
                TypeHelper.MethodName<IServiceProvider, Type, object>(p => p.GetService),
                new CodeTypeOfExpression(new CodeTypeReference(codeObject.TargetType))
            ));
        }
    }
}
