using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomGeneratorServiceContext : DefaultCodeGeneratorServiceContext, INamingContext
    {
        public INaming Naming { get; set; }

        public CodeDomGeneratorServiceContext(INaming naming, TextWriter output, IDependencyProvider dependencyProvider, ICollection<IErrorInfo> errors = null)
            : base(output, dependencyProvider, errors)
        {
            Naming = naming;
        }

        public override ICodeGeneratorContext CreateGeneratorContext(ICodeGeneratorService service)
        {
            return new CodeDomGeneratorContext(Naming, Output, service, DependencyProvider, Errors);
        }
    }
}
