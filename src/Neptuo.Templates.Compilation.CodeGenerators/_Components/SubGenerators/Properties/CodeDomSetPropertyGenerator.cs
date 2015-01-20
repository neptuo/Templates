using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomSetPropertyGenerator : CodeDomPropertyGeneratorBase<SetCodeProperty>
    {
        protected override ICodeDomPropertyResult Generate(ICodeDomPropertyContext context, SetCodeProperty codeProperty)
        {
            CodeDomDefaultPropertyResult statements = new CodeDomDefaultPropertyResult();

            bool isWriteable = !codeProperty.Property.IsReadOnly;
            Type targetItemType = codeProperty.Property.Type;

            // Expression for accessing target property.
            CodeExpression targetField = context.PropertyTarget;
            CodeExpression codePropertyReference = new CodePropertyReferenceExpression(
                targetField,
                codeProperty.Property.Name
            );

            // Resolve code object value.
            ICodeDomObjectResult result = context.Registry.WithObjectGenerator().Generate(
                context.CreateObjectContext().AddCodeProperty(codeProperty).AddPropertyTarget(context.PropertyTarget),
                codeProperty.Value
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
                    result.ExpressionReturnType
                );

                if (expression == null)
                    return null;

                // Add statement to the collection.
                statements.AddStatement(new CodeAssignStatement(
                    codePropertyReference,
                    expression
                ));
            }

            return statements;
        }
    }
}
