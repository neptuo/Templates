using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators.Extensions.CodeDom
{
    public class SetPropertyDescriptorExtension : BasePropertyDescriptorExtension<SetPropertyDescriptor>
    {
        protected override void GenerateProperty(PropertyDescriptorExtensionContext context, SetPropertyDescriptor propertyDescriptor)
        {
            CodeExpression codeExpression = context.Context.CodeGenerator.GenerateCodeObject(context.Context, propertyDescriptor.Value, propertyDescriptor, context.BindMethod, context.FieldName);
            context.BindMethod.Statements.Add(
                new CodeAssignStatement(
                    new CodePropertyReferenceExpression(
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(),
                            context.FieldName
                        ),
                        propertyDescriptor.PropertyName.Name
                    ),
                    codeExpression
                )
            );
        }
    }
}
