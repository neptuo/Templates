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

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="variableName">Name of the variable in bind method.</param>
        public CodeDomRootObjectGenerator(string variableName)
        {
            Guard.NotNullOrEmpty(variableName, "variableName");
            this.variableName = variableName;
        }

        protected override ICodeDomObjectResult Generate(ICodeDomObjectContext context, RootCodeObject codeObject)
        {
            CodeDomAstPropertyFeature generator = new CodeDomAstPropertyFeature();
            IEnumerable<CodeStatement> statements = generator.Generate(context, codeObject, variableName);
            if (statements == null)
                return null;

            context.Structure.EntryPoint.Statements.AddRange(statements);
            return new CodeDomDefaultObjectResult();
        }
    }
}
