using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Default implementation of <see cref="ICodeDomContext"/>.
    /// </summary>
    public class DefaultCodeDomContext : ICodeDomContext
    {
        public ICodeGeneratorContext GeneratorContext { get; private set; }
        public ICodeDomConfiguration Configuration { get; private set; }
        public ICodeDomStructure Structure { get; private set; }
        public ICodeDomRegistry Registry { get; private set; }

        public DefaultCodeDomContext(ICodeGeneratorContext generatorContext, ICodeDomConfiguration configuration, ICodeDomStructure structure, ICodeDomRegistry registry)
        {
            Guard.NotNull(generatorContext, "generatorContext");
            Guard.NotNull(configuration, "configuration");
            Guard.NotNull(structure, "structure");
            Guard.NotNull(registry, "registry");
            GeneratorContext = generatorContext;
            Configuration = configuration;
            Structure = structure;
            Registry = registry;
        }
    }
}
