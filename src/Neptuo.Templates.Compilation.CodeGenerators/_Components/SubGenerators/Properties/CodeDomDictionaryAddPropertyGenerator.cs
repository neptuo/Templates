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
    /// <summary>
    /// Generator for properties which should use <see cref="Dictionary.Add"/> method.
    /// </summary>
    public class CodeDomDictionaryAddPropertyGenerator : CodeDomPropertyGeneratorBase<DictionaryAddCodeProperty>
    {
        /// <summary>
        /// Name of the <see cref="Dictionary.Add"/> method.
        /// </summary>
        private static string addMethodName = TypeHelper.MethodName<IDictionary<string, string>, string, string>(d => d.Add);

        protected override ICodeDomPropertyResult Generate(ICodeDomPropertyContext context, DictionaryAddCodeProperty codeProperty)
        {
            CodeDomDefaultPropertyResult statements = new CodeDomDefaultPropertyResult();

            bool isWriteable = !codeProperty.Property.IsReadOnly;
            Type targetKeyItemType = typeof(object);
            Type targetValueItemType = typeof(object);

            // Expression for accessing target property.
            CodeExpression targetField = context.PropertyTarget;
            CodeExpression codePropertyReference = new CodePropertyReferenceExpression(
                targetField,
                codeProperty.Property.Name
            );

            // Try to get target property type.
            if (codeProperty.Property.Type.IsGenericType)
            {
                Type[] genericArguments = codeProperty.Property.Type.GetGenericArguments();
                if (genericArguments.Length == 2)
                {
                    targetKeyItemType = genericArguments[0];
                    targetValueItemType = genericArguments[1];
                }
            }

            // If writeable, create new instance.
            if (isWriteable)
            {
                statements.AddStatement(
                    new CodeAssignStatement(
                        codePropertyReference,
                        new CodeObjectCreateExpression(codeProperty.Property.Type)
                    )
                );
            }

            // Foreach property value...
            foreach (KeyValuePair<ICodeObject, ICodeObject> propertyValue in codeProperty.Values)
            {
                // Resolve key code object value.
                ICodeDomObjectResult keyResult = context.Registry.WithObjectGenerator().Generate(
                    context.CreateObjectContext().AddCodeProperty(codeProperty).AddPropertyTarget(context.PropertyTarget),
                    propertyValue.Key
                );

                // Resolve value code object value.
                ICodeDomObjectResult valueResult = context.Registry.WithObjectGenerator().Generate(
                    context.CreateObjectContext().AddCodeProperty(codeProperty).AddPropertyTarget(context.PropertyTarget),
                    propertyValue.Value
                );

                // If result is null, something wrong.
                if (keyResult == null || valueResult == null)
                    return null;

                // If result has expression...
                if (keyResult.HasExpression() && valueResult.HasExpression())
                {
                    // Try to convert to key type.
                    CodeExpression keyExpression = context.Registry.WithConversionGenerator().Generate(
                        context,
                        targetKeyItemType,
                        keyResult.Expression,
                        keyResult.ExpressionReturnType
                    );

                    // Try to convert to value type.
                    CodeExpression valueExpression = context.Registry.WithConversionGenerator().Generate(
                        context,
                        targetValueItemType,
                        valueResult.Expression,
                        valueResult.ExpressionReturnType
                    );

                    if (keyExpression == null || valueExpression == null)
                        return null;

                    // Add statement to the collection.
                    statements.AddStatement(
                        new CodeExpressionStatement(
                            new CodeMethodInvokeExpression(
                                codePropertyReference,
                                addMethodName,
                                keyExpression,
                                valueExpression
                            )
                        )
                    );
                }
            }
            return statements;
        }
    }
}
