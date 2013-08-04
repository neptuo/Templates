using Neptuo.Templates.Compilation.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class DefaultCodeGeneratorServiceContext : ICodeGeneratorServiceContext
    {
        public TextWriter Output { get; set; }
        public IDependencyProvider DependencyProvider { get; set; }
        public ICollection<IErrorInfo> Errors { get; set; }

        public DefaultCodeGeneratorServiceContext(TextWriter output, IDependencyProvider dependencyProvider, ICollection<IErrorInfo> errors = null)
        {
            Output = output;
            DependencyProvider = dependencyProvider;
            Errors = errors ?? new List<IErrorInfo>();
        }

        public virtual ICodeGeneratorContext CreateGeneratorContext(ICodeGeneratorService service)
        {
            IDependencyContainer provider = DependencyProvider.CreateChildContainer();
            provider.RegisterInstance<StorageProvider>(new StorageProvider());
            return new DefaultCodeGeneratorContext(Output, service, provider, Errors);
        }
    }
}
