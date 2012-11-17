using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions.CodeDom
{
    public class MethodInvokePropertyDescriptorExtension : BasePropertyDescriptorExtension<MethodInvokePropertyDescriptor>
    {
        protected override void GenerateProperty(PropertyDescriptorExtensionContext context, MethodInvokePropertyDescriptor propertyDescriptor)
        {
            context.BindMethod.Statements.Add(
                new CodeMethodInvokeExpression(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        context.FieldName
                    ),
                    propertyDescriptor.Method.Name,
                    propertyDescriptor.Parameters.Select(p => new CodePrimitiveExpression(((PlainValueCodeObject)p).Value)).ToArray()
                )
            );
        }
    }
}
