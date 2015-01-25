using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.Compilation.CodeGenerators
{
    public class CodeDomAstObserverFeature
    {
        public IEnumerable<CodeStatement> Generate(ICodeDomContext context, IObserversCodeObject codeObject, string variableName)
        {
            List<CodeStatement> statements = new List<CodeStatement>();
            foreach (IObserverCodeObject observer in codeObject.Observers)
            {
                ICodeDomObjectResult result = context.Registry.WithObjectGenerator().Generate(
                    context.CreateObjectContext().AddObserverTarget(variableName), 
                    observer
                );
                if (result == null)
                    return null;

                if (result.HasExpression())
                    statements.Add(new CodeExpressionStatement(result.Expression));
            }

            return statements;
        }
    }
}
