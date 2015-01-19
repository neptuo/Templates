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
            DefaultCodeDomPropertyResult statements = null;
            ICodeDomObjectResult valueResult = context.Registry.WithObjectGenerator().Generate(
                context.CreateObjectContext(),
                codeProperty.Value
            );
            if (valueResult != null)
            {
                statements = new DefaultCodeDomPropertyResult();
                if(valueResult.HasExpression())
                {
                    statements.AddStatement(new CodeAssignStatement(
                        new CodePropertyReferenceExpression(
                            context.PropertyTarget,
                            codeProperty.Property.Name
                        ),
                        valueResult.Expression
                    ));
                }
            }

            return statements;
        }
    }
}
