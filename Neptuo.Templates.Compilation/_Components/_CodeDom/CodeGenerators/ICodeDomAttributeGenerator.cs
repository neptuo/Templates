using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Generates code for properties decorated with attribute, but without assigned value.
    /// </summary>
    public interface ICodeDomAttributeGenerator
    {
        /// <summary>
        /// Generates code for properties decorated with <paramref name="attribute"/>, but without assigned value.
        /// </summary>
        /// <param name="context">Context for code generation.</param>
        /// <param name="attribute">Attribute instance.</param>
        /// <returns>Generated properties value assigment.</returns>
        CodeExpression GenerateCode(CodeDomAttributeContext context, Attribute attribute);
    }
}
