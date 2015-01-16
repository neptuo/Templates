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
        /// Extensible registry for generators.
        /// </summary>
        ICodeDomRegistry GeneratorRegistry { get; }
    }
}
