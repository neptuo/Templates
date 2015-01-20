using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Base context for code dom generators.
    /// </summary>
    public interface ICodeDomContext
    {
        /// <summary>
        /// Code generator context passed to the <see cref="ICodeGenerator"/>.
        /// </summary>
        ICodeGeneratorContext GeneratorContext { get; }

        /// <summary>
        /// Configuration.
        /// </summary>
        ICodeDomConfiguration Configuration { get; }

        /// <summary>
        /// Base structure for generated code.
        /// </summary>
        ICodeDomStructure Structure { get; }

        /// <summary>
        /// Extensible registry for generators.
        /// </summary>
        ICodeDomRegistry Registry { get; }
    }
}
