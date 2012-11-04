using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Utils;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions
{
    public class ListAddCodeDomPropertyDescriptorExtension : BaseCodeDomPropertyDescriptorExtension<ListAddPropertyDescriptor>
    {
        protected override void GenerateProperty(CodeDomPropertyDescriptorExtensionContext context, ListAddPropertyDescriptor propertyDescriptor)
        {
            //Create instance
            //TODO: To support IEnumerable, add casting!
            if (typeof(ICollection<>).IsAssignableFrom(propertyDescriptor.Property.PropertyType.GetGenericTypeDefinition()))
            {
                context.BindMethod.Statements.Add(
                    new CodeAssignStatement(
                        new CodePropertyReferenceExpression(
                            new CodeFieldReferenceExpression(
                                new CodeThisReferenceExpression(),
                                context.FieldName
                            ),
                            propertyDescriptor.Property.Name
                        ),
                        new CodeObjectCreateExpression(
                            typeof(List<>).MakeGenericType(propertyDescriptor.Property.PropertyType.GetGenericArguments()[0])
                        )
                    )
                );
            }

            foreach (ICodeObject propertyValue in propertyDescriptor.Values)
            {
                CodeExpression codeExpression = context.CodeGenerator.GenerateCodeObject(propertyValue, propertyDescriptor, context.BindMethod, context.FieldName);
                if (typeof(ICollection<>).IsAssignableFrom(propertyDescriptor.Property.PropertyType.GetGenericTypeDefinition()))
                {
                    context.BindMethod.Statements.Add(
                        new CodeMethodInvokeExpression(
                            new CodePropertyReferenceExpression(
                                new CodeFieldReferenceExpression(
                                    new CodeThisReferenceExpression(),
                                    context.FieldName
                                ),
                                propertyDescriptor.Property.Name
                            ),
                            TypeHelper.MethodName<ICollection<object>, object>(c => c.Add),
                            codeExpression
                        )
                    );
                }
                //TODO: Other bindable ways
            }
        }
    }
}
