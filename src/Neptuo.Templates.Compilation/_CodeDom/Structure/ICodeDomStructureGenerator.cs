using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Generator for base structure of complited template.
    /// <see cref="ICodeDomStructure"/>.
    /// </summary>
    public interface ICodeDomStructureGenerator
    {
        /// <summary>
        /// Genetes base structure for compiled template.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <returns>Base structure for compiled template.</returns>
        ICodeDomStructure Generate(ICodeGeneratorContext context);
    }
}
