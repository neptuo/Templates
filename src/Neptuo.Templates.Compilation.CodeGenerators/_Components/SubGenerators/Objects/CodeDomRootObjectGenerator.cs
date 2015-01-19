using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomRootObjectGenerator : CodeDomObjectGeneratorBase<RootCodeObject>
    {
        private readonly string variableName;

        public CodeDomRootObjectGenerator(string variableName)
        {
            Guard.NotNullOrEmpty(variableName, "variableName");
            this.variableName = variableName;
        }

        protected override ICodeDomObjectResult Generate(ICodeDomObjectContext context, RootCodeObject codeObject)
        {
            CodeDomObjectPropertyGenerator generator = new CodeDomObjectPropertyGenerator();
            IEnumerable<CodeStatement> statements = generator.Generate(context, codeObject, "view");
            if (statements == null)
                return null;

            context.Structure.EntryPoint.Statements.AddRange(statements);
            return new DefaultCodeDomObjectResult();
        }
    }
}
