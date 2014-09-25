using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Default implementation of <see cref="ICodeGeneratorContext"/>.
    /// </summary>
    public class DefaultCodeGeneratorContext : ICodeGeneratorContext
    {
        public TextWriter Output { get; private set; }

        public ICodeGeneratorService CodeGeneratorService { get; private set; }
        public IDependencyProvider DependencyProvider { get; private set; }
        public ICollection<IErrorInfo> Errors { get; private set; }

        public DefaultCodeGeneratorContext(TextWriter output, ICodeGeneratorService generatorService, IDependencyProvider dependencyProvider, ICollection<IErrorInfo> errors = null)
        {
            Guard.NotNull(output, "output");
            Guard.NotNull(generatorService, "generatorService");
            Guard.NotNull(dependencyProvider, "dependencyProvider");
            Output = output;
            CodeGeneratorService = generatorService;
            DependencyProvider = dependencyProvider;
            Errors = errors ?? new List<IErrorInfo>();
        }
    }
}
