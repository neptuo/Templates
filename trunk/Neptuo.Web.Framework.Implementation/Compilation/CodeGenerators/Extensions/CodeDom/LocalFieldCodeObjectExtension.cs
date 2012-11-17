using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions.CodeDom
{
    public class LocalFieldCodeObjectExtension : BaseCodeObjectExtension<LocalFieldCodeObject>
    {
        protected override CodeExpression GenerateCode(CodeObjectExtensionContext context, LocalFieldCodeObject codeObject, IPropertyDescriptor propertyDescriptor)
        {
            return new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(),
                codeObject.FieldName
            );
        }
    }
}
