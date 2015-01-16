using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Describes context for base structure code generation.
    /// </summary>
    public class CodeDomStructureContext
    {
        /// <summary>
        /// Inner context of code dom generator.
        /// </summary>
        public XCodeDomGenerator.Context GeneratorContext { get; private set; }

        /// <summary>
        /// Current class naming strategy.
        /// </summary>
        public INaming Naming { get; private set; }

        public CodeDomStructureContext(XCodeDomGenerator.Context generatorContext, INaming naming)
        {
            Guard.NotNull(generatorContext, "generatorContext");
            Guard.NotNull(naming, "naming");
            GeneratorContext = generatorContext;
            Naming = naming;
        }
    }
}
