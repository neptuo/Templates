using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class XCodeDomDictionaryAddPropertyGenerator : XCodeDomPropertyGeneratorBase<DictionaryAddCodeProperty>
    {
        private static string addMethodName = TypeHelper.MethodName<IDictionary<string, string>, string, string>(d => d.Add);

        public XCodeDomDictionaryAddPropertyGenerator(Type requiredComponentType, ComponentManagerDescriptor componentManagerDescriptor)
            : base(requiredComponentType, componentManagerDescriptor)
        { }

        protected override void GenerateProperty(CodeDomPropertyContext context, DictionaryAddCodeProperty codeProperty)
        {
            bool isWriteable = !codeProperty.Property.IsReadOnly;

            CodeExpression targetField = GetPropertyTarget(context);
            CodeExpression codePropertyReference = new CodePropertyReferenceExpression(
                targetField,
                codeProperty.Property.Name
            );

            if (isWriteable)
            {
                context.Statements.Add(
                    new CodeAssignStatement(
                        codePropertyReference,
                        new CodeObjectCreateExpression(codeProperty.Property.Type)
                    )
                );
            }

            foreach (KeyValuePair<ICodeObject, ICodeObject> propertyValue in codeProperty.Values)
            {
                CodeExpression keyCodeExpression = context.CodeGenerator.GenerateCodeObject(
                    new CodeObjectExtensionContext(context.Context, context.FieldName),
                    propertyValue.Key,
                    codeProperty
                );
                CodeExpression valueCodeExpression = context.CodeGenerator.GenerateCodeObject(
                    new CodeObjectExtensionContext(context.Context, context.FieldName),
                    propertyValue.Value,
                    codeProperty
                );
                if (keyCodeExpression != null && valueCodeExpression != null)
                {
                    context.Statements.Add(
                        new CodeMethodInvokeExpression(
                            codePropertyReference,
                            addMethodName,
                            keyCodeExpression,
                            valueCodeExpression
                        )
                    );
                }
            }
        }
    }
}
