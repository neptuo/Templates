using Neptuo.Activators;
using Neptuo.ComponentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Implementation of <see cref="ICodeGeneratorServiceContext"/>
    /// </summary>
    public class DefaultCodeGeneratorServiceContext : ICodeGeneratorServiceContext
    {
        public TextWriter Output { get; private set; }
        public IDependencyProvider DependencyProvider { get; private set; }
        public ICollection<IErrorInfo> Errors { get; private set; }

        public DefaultCodeGeneratorServiceContext(TextWriter output, IDependencyProvider dependencyProvider, ICollection<IErrorInfo> errors = null)
        {
            Ensure.NotNull(output, "output");
            Ensure.NotNull(dependencyProvider, "dependencyProvider");
            Output = output;
            DependencyProvider = dependencyProvider;
            Errors = errors ?? new List<IErrorInfo>();
        }

        public virtual ICodeGeneratorContext CreateGeneratorContext(ICodeGeneratorService service)
        {
            Ensure.NotNull(service, "service");
            return new DefaultCodeGeneratorContext(Output, service, DependencyProvider, Errors);
        }
    }
}
