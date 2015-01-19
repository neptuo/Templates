using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomObjectPropertyGenerator
    {
        public IEnumerable<CodeStatement> Generate(ICodeDomContext context, IPropertiesCodeObject codeObject, string variableName)
        {
            Type componentType = null;
            ITypeCodeObject typeCodeObject = codeObject as ITypeCodeObject;
            if (typeCodeObject != null)
                componentType = typeCodeObject.Type;

            List<CodeStatement> statements = new List<CodeStatement>();
            CodeExpression variableExpression = new CodeVariableReferenceExpression(variableName);
            foreach (ICodeProperty codeProperty in codeObject.Properties)
            {
                ICodeDomPropertyResult result = context.Registry.WithPropertyGenerator().Generate(
                    context.CreatePropertyContext(variableExpression),
                    codeProperty
                );

                if (result == null)
                    return null;

                statements.AddRange(result.Statements);
            }

            return statements;
        }
    }
}
