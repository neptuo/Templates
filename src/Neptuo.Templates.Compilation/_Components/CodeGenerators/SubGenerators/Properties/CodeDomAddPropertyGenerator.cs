using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Generator for properties which should use <see cref="List{T}.Add"/> method.
    /// </summary>
    public class CodeDomAddPropertyGenerator : CodeDomPropertyGeneratorBase<AddCodeProperty>
    {
        /// <summary>
        /// Name of the <see cref="List{T}.Add"/> method.
        /// </summary>
        private static string addMethodName = TypeHelper.MethodName<ICollection<object>, object>(c => c.Add);

        protected override ICodeDomPropertyResult Generate(ICodeDomPropertyContext context, AddCodeProperty codeProperty)
        {
            CodeDomDefaultPropertyResult statements = new CodeDomDefaultPropertyResult();

            bool isGenericProperty = codeProperty.Type.IsGenericType;
            bool isCastingRequired = false;
            bool isWriteable = false;
            Type targetType = null;
            Type targetItemType = typeof(object);

            // Expression for accessing target property.
            CodeExpression targetField = context.PropertyTarget;
            CodeExpression codePropertyReference = new CodePropertyReferenceExpression(
                targetField,
                codeProperty.Name
            );

            Type declaringType;
            if (context.TryGetFieldType(out declaringType))
            {
                PropertyInfo propertyInfo = declaringType.GetProperty(codeProperty.Name);
                if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                    isWriteable = propertyInfo.CanWrite;
            }

            // Try to get target property type.
            if (typeof(IEnumerable).IsAssignableFrom(codeProperty.Type))
            {
                isCastingRequired = true;
                if (isGenericProperty)
                {
                    targetItemType = codeProperty.Type.GetGenericArguments()[0];
                    targetType = typeof(List<>).MakeGenericType(targetItemType);

                    if (typeof(ICollection<>).IsAssignableFrom(codeProperty.Type.GetGenericTypeDefinition()))
                        isCastingRequired = false;
                }
                else
                {
                    targetType = typeof(List<object>);
                }
            }

            // If writeable, create new instance.
            if (isWriteable)
            {
                statements.AddStatement(
                    new CodeAssignStatement(
                        codePropertyReference,
                        new CodeObjectCreateExpression(targetType)
                    )
                );
            }

            // Is adding items will required casting (eg.: we created instance of type List<T>, but property is of type IEnumerable<T>).
            if (isCastingRequired)
                codePropertyReference = new CodeCastExpression(targetType, codePropertyReference);

            // Foreach property value...
            foreach (ICodeObject propertyValue in codeProperty.Values)
            {
                // Resolve code object value.
                ICodeDomObjectResult result = context.Registry.WithObjectGenerator().Generate(
                    context.CreateObjectContext().AddCodeProperty(codeProperty).AddPropertyTarget(context.PropertyTarget),
                    propertyValue
                );

                // If result is null, something wrong.
                if (result == null)
                    return null;

                // If result has expression...
                if (result.HasExpression())
                {
                    // Try to convert to collection type.
                    CodeExpression expression = context.Registry.WithConversionGenerator().Generate(
                        context, 
                        targetItemType, 
                        result.Expression, 
                        result.Expression.GetReturnType()
                    );

                    if (expression == null)
                        return null;

                    // Add statement to the collection.
                    statements.AddStatement(
                        new CodeExpressionStatement(
                            new CodeMethodInvokeExpression(
                                codePropertyReference,
                                addMethodName,
                                expression
                            )
                        )
                    );
                }
                else if(result.HasStatement())
                {
                    // If result has statement (possibly comment), add it to the result.
                    statements.AddStatement(result.Statement);
                }
            }

            // Return statements.
            return statements;
        }
    }
}
