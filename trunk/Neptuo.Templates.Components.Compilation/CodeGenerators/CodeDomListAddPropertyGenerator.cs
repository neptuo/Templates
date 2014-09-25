using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomListAddPropertyGenerator : CodeDomPropertyGeneratorBase<ListAddPropertyDescriptor>
    {
        protected override void GenerateProperty(CodeDomPropertyContext context, ListAddPropertyDescriptor propertyDescriptor)
        {
            bool generic = propertyDescriptor.Property.Type.IsGenericType;
            bool requiresCasting = false;
            bool createInstance = !propertyDescriptor.Property.IsReadOnly;
            Type targetType = null;
            string addMethodName = null;

            CodeExpression targetField = GetPropertyTarget(context);
            CodeExpression codePropertyReference = new CodePropertyReferenceExpression(
                targetField,
                propertyDescriptor.Property.Name
            );

            if (typeof(IEnumerable).IsAssignableFrom(propertyDescriptor.Property.Type))
            {
                requiresCasting = true;
                if (generic)
                {
                    targetType = typeof(List<>).MakeGenericType(propertyDescriptor.Property.Type.GetGenericArguments()[0]);
                    addMethodName = TypeHelper.MethodName<ICollection<object>, object>(c => c.Add);

                    if (typeof(ICollection<>).IsAssignableFrom(propertyDescriptor.Property.Type.GetGenericTypeDefinition()))
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
                context.Statements.Add(
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
                CodeExpression codeExpression = context.CodeGenerator.GenerateCodeObject(
                    new CodeObjectExtensionContext(context.Context, context.FieldName), 
                    propertyValue, 
                    propertyDescriptor
                );
                context.Statements.Add(
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
