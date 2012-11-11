using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions
{
    public class LocalFieldCodeDomCodeObjectExtension : BaseCodeDomCodeObjectExtension<LocalFieldCodeObject>
    {
        protected override CodeExpression GenerateCode(CodeDomCodeObjectExtensionContext context, LocalFieldCodeObject codeObject, IPropertyDescriptor propertyDescriptor)
        {
            return new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(),
                codeObject.FieldName
            );
        }
    }
}
