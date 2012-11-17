using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Extensions;
using Neptuo.Web.Framework.Utils;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions
{
    public class ExtensionCodeDomCodeObjectExtension : BaseComponentCodeDomCodeObjectExtension<ExtensionCodeObject>
    {
        protected override CodeExpression GenerateCode(CodeDomCodeObjectExtensionContext context, ExtensionCodeObject codeObject, IPropertyDescriptor propertyDescriptor)
        {
            CodeFieldReferenceExpression field = GenerateCompoment(context, codeObject, codeObject);
            
            context.ParentBindMethod.Statements.Add(
                new CodeMethodInvokeExpression(
                    new CodeThisReferenceExpression(),
                    context.CodeGenerator.FormatBindMethod(field.FieldName)
                )
            );

            return new CodeCastExpression(
                propertyDescriptor.Property.PropertyType,
                new CodeMethodInvokeExpression(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        field.FieldName
                    ),
                    TypeHelper.MethodName<IMarkupExtension, object>(m => m.ProvideValue)
                )
            );
        }
    }
}
