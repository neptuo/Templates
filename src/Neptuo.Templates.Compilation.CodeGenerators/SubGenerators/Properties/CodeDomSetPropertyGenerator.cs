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
        protected override CodeStatement Generate(ICodeDomPropertyContext context, SetCodeProperty codeProperty)
        {
            ICodeDomObjectResult valueResult = context.Registry.WithObjectGenerator().Generate(
                context,
                codeProperty.Value
            );
            if (valueResult != null)
            {
                return new CodeAssignStatement(
                    new CodePropertyReferenceExpression(
                        context.PropertyTarget,
                        codeProperty.Property.Name
                    ),
                    valueResult.Expression
                );
            }

            return null;
        }
    }
}
