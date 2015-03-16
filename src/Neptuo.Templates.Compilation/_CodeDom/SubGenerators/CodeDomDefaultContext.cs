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
    public class CodeDomDefaultContext : ICodeDomContext
    {
        public ICodeGeneratorContext GeneratorContext { get; private set; }
        public ICodeDomConfiguration Configuration { get; private set; }
        public ICodeDomStructure Structure { get; private set; }
        public ICodeDomRegistry Registry { get; private set; }

        public CodeDomDefaultContext(ICodeGeneratorContext generatorContext, ICodeDomConfiguration configuration, ICodeDomStructure structure, ICodeDomRegistry registry)
        {
            Ensure.NotNull(generatorContext, "generatorContext");
            Ensure.NotNull(configuration, "configuration");
            Ensure.NotNull(structure, "structure");
            Ensure.NotNull(registry, "registry");
            GeneratorContext = generatorContext;
            Configuration = configuration;
            Structure = structure;
            Registry = registry;
        }
    }
}
