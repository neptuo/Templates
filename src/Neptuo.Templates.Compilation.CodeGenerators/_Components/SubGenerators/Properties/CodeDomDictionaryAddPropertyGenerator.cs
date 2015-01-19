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
    public class CodeDomDictionaryAddPropertyGenerator : CodeDomPropertyGeneratorBase<DictionaryAddCodeProperty>
    {
        private static string addMethodName = TypeHelper.MethodName<IDictionary<string, string>, string, string>(d => d.Add);

        protected override ICodeDomPropertyResult Generate(ICodeDomPropertyContext context, DictionaryAddCodeProperty codeProperty)
        {
            DefaultCodeDomPropertyResult statements = new DefaultCodeDomPropertyResult();

            bool isWriteable = !codeProperty.Property.IsReadOnly;

            CodeExpression targetField = context.PropertyTarget;
            CodeExpression codePropertyReference = new CodePropertyReferenceExpression(
                targetField,
                codeProperty.Property.Name
            );

            if (isWriteable)
            {
                statements.AddStatement(
                    new CodeAssignStatement(
                        codePropertyReference,
                        new CodeObjectCreateExpression(codeProperty.Property.Type)
                    )
                );
            }

            foreach (KeyValuePair<ICodeObject, ICodeObject> propertyValue in codeProperty.Values)
            {
                ICodeDomObjectResult keyResult = context.Registry.WithObjectGenerator().Generate(
                    context.CreateObjectContext().AddCodeProperty(codeProperty),
                    propertyValue.Key
                );
                ICodeDomObjectResult valueResult = context.Registry.WithObjectGenerator().Generate(
                    context.CreateObjectContext().AddCodeProperty(codeProperty),
                    propertyValue.Value
                );

                if (keyResult == null || valueResult == null)
                    return null;

                statements.AddStatement(
                    new CodeExpressionStatement(
                        new CodeMethodInvokeExpression(
                            codePropertyReference,
                            addMethodName,
                            keyResult.Expression,
                            valueResult.Expression
                        )
                    )
                );
            }
            return statements;
        }
    }
}
