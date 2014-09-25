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
    public class CodeDomDictionaryAddPropertyGenerator : CodeDomPropertyGeneratorBase<DictionaryAddPropertyDescriptor>
    {
        private static string addMethodName = TypeHelper.MethodName<IDictionary<string, string>, string, string>(d => d.Add);

        protected override void GenerateProperty(CodeDomPropertyContext context, DictionaryAddPropertyDescriptor propertyDescriptor)
        {
            bool createInstance = !propertyDescriptor.Property.IsReadOnly;

            CodeExpression targetField = GetPropertyTarget(context);
            CodeExpression codePropertyReference = new CodePropertyReferenceExpression(
                targetField,
                propertyDescriptor.Property.Name
            );

            if (createInstance)
            {
                context.Statements.Add(
                    new CodeAssignStatement(
                        codePropertyReference,
                        new CodeObjectCreateExpression(propertyDescriptor.Property.Type)
                    )
                );
            }

            foreach (KeyValuePair<ICodeObject, ICodeObject> propertyValue in propertyDescriptor.Values)
            {
                CodeExpression keyCodeExpression = context.CodeGenerator.GenerateCodeObject(
                    new CodeObjectExtensionContext(context.Context, context.FieldName),
                    propertyValue.Key,
                    propertyDescriptor
                );
                CodeExpression valueCodeExpression = context.CodeGenerator.GenerateCodeObject(
                    new CodeObjectExtensionContext(context.Context, context.FieldName),
                    propertyValue.Value,
                    propertyDescriptor
                );
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
