using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Utils;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions.CodeDom
{
    public class ListAddPropertyDescriptorExtension : BasePropertyDescriptorExtension<ListAddPropertyDescriptor>
    {
        protected override void GenerateProperty(PropertyDescriptorExtensionContext context, ListAddPropertyDescriptor propertyDescriptor)
        {
            bool generic = propertyDescriptor.Property.PropertyType.IsGenericType;
            bool requiresCasting = false;
            bool createInstance = propertyDescriptor.Property.GetSetMethod() != null;
            Type targetType = null;
            string addMethodName = null;

            CodeExpression codePropertyReference = new CodePropertyReferenceExpression(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    context.FieldName
                ),
                propertyDescriptor.Property.Name
            );

            if (typeof(IEnumerable).IsAssignableFrom(propertyDescriptor.Property.PropertyType))
            {
                requiresCasting = true;
                if (generic)
                {
                    targetType = typeof(List<>).MakeGenericType(propertyDescriptor.Property.PropertyType.GetGenericArguments()[0]);
                    addMethodName = TypeHelper.MethodName<ICollection<object>, object>(c => c.Add);

                    if (typeof(ICollection<>).IsAssignableFrom(propertyDescriptor.Property.PropertyType.GetGenericTypeDefinition()))
                        requiresCasting = false;
                }
                else
                {
                    targetType = typeof(List<object>);
                    addMethodName = TypeHelper.MethodName<List<object>, object>(c => c.Add);
                }
            }

            if (createInstance)
            {
                context.BindMethod.Statements.Add(
                    new CodeAssignStatement(
                        codePropertyReference,
                        new CodeObjectCreateExpression(targetType)
                    )
                );
            }

            if (requiresCasting)
                codePropertyReference = new CodeCastExpression(targetType, codePropertyReference);

            foreach (ICodeObject propertyValue in propertyDescriptor.Values)
            {
                CodeExpression codeExpression = context.CodeGenerator.GenerateCodeObject(propertyValue, propertyDescriptor, context.BindMethod, context.FieldName);
                
                context.BindMethod.Statements.Add(
                    new CodeMethodInvokeExpression(
                        codePropertyReference,
                        addMethodName,
                        codeExpression
                    )
                );
                //TODO: Other bindable ways
            }
        }
    }
}
