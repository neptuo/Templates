using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomListAddPropertyGenerator : CodeDomPropertyGeneratorBase<ListAddCodeProperty>
    {
        protected override ICodeDomPropertyResult Generate(ICodeDomPropertyContext context, ListAddCodeProperty codeProperty)
        {
            bool isGenericProperty = codeProperty.Property.Type.IsGenericType;
            bool isCastingRequired = false;
            bool isWriteable = !codeProperty.Property.IsReadOnly;
            Type targetType = null;
            string addMethodName = null;

            CodeExpression targetField = context.PropertyTarget;
            CodeExpression codePropertyReference = new CodePropertyReferenceExpression(
                targetField,
                codeProperty.Property.Name
            );

            List<CodeStatement> statements = new List<CodeStatement>();
            if (typeof(IEnumerable).IsAssignableFrom(codeProperty.Property.Type))
            {
                isCastingRequired = true;
                if (isGenericProperty)
                {
                    targetType = typeof(List<>).MakeGenericType(codeProperty.Property.Type.GetGenericArguments()[0]);
                    addMethodName = TypeHelper.MethodName<ICollection<object>, object>(c => c.Add);

                    if (typeof(ICollection<>).IsAssignableFrom(codeProperty.Property.Type.GetGenericTypeDefinition()))
                        isCastingRequired = false;
                }
                else
                {
                    targetType = typeof(List<object>);
                    addMethodName = TypeHelper.MethodName<List<object>, object>(c => c.Add);
                }
            }

            if (isWriteable)
            {
                statements.Add(
                    new CodeAssignStatement(
                        codePropertyReference,
                        new CodeObjectCreateExpression(targetType)
                    )
                );
            }

            if (isCastingRequired)
                codePropertyReference = new CodeCastExpression(targetType, codePropertyReference);

            foreach (ICodeObject propertyValue in codeProperty.Values)
            {
                ICodeDomObjectResult result = context.Registry.WithObjectGenerator().Generate(
                    context.CreateObjectContext().AddCodeProperty(codeProperty),
                    propertyValue
                );

                if (result == null)
                    return null;

                if (result.HasExpression())
                {
                    statements.Add(
                        new CodeExpressionStatement(
                            new CodeMethodInvokeExpression(
                                codePropertyReference,
                                addMethodName,
                                result.Expression
                            )
                        )
                    );
                }
                //TODO: Other bindable ways
            }

            return new DefaultCodeDomPropertyResult(statements);
        }
    }
}
