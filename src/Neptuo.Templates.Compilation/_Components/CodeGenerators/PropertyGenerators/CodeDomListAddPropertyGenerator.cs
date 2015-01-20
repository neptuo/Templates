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
    public class XCodeDomListAddPropertyGenerator : XCodeDomPropertyGeneratorBase<ListAddCodeProperty>
    {
        public XCodeDomListAddPropertyGenerator(Type requiredComponentType, ComponentManagerDescriptor componentManagerDescriptor)
            : base(requiredComponentType, componentManagerDescriptor)
        { }

        protected override void GenerateProperty(CodeDomPropertyContext context, ListAddCodeProperty codeProperty)
        {
            bool generic = codeProperty.Property.Type.IsGenericType;
            bool requiresCasting = false;
            bool createInstance = !codeProperty.Property.IsReadOnly;
            Type targetType = null;
            string addMethodName = null;

            CodeExpression targetField = GetPropertyTarget(context);
            CodeExpression codePropertyReference = new CodePropertyReferenceExpression(
                targetField,
                codeProperty.Property.Name
            );

            if (typeof(IEnumerable).IsAssignableFrom(codeProperty.Property.Type))
            {
                requiresCasting = true;
                if (generic)
                {
                    targetType = typeof(List<>).MakeGenericType(codeProperty.Property.Type.GetGenericArguments()[0]);
                    addMethodName = TypeHelper.MethodName<ICollection<object>, object>(c => c.Add);

                    if (typeof(ICollection<>).IsAssignableFrom(codeProperty.Property.Type.GetGenericTypeDefinition()))
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

            foreach (ICodeObject propertyValue in codeProperty.Values)
            {
                CodeExpression codeExpression = context.CodeGenerator.GenerateCodeObject(
                    new CodeObjectExtensionContext(context.Context, context.FieldName), 
                    propertyValue, 
                    codeProperty
                );
                if (codeExpression != null)
                {
                    context.Statements.Add(
                        new CodeMethodInvokeExpression(
                            codePropertyReference,
                            addMethodName,
                            codeExpression
                        )
                    );
                }
                //TODO: Other bindable ways
            }
        }
    }
}
