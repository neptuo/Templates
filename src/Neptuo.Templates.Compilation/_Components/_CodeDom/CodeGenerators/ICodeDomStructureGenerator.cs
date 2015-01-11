using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Generator for base structure of generated class/unit.
    /// </summary>
    public interface ICodeDomStructureGenerator
    {
        /// <summary>
        /// Generates code for base structure of generated class/unit.
        /// </summary>
        /// <param name="context">Context of code generation.</param>
        /// <returns>Base structure of generated class/unit.</returns>
        CodeDomStructure GenerateCode(CodeDomStructureContext context);
    }
}
