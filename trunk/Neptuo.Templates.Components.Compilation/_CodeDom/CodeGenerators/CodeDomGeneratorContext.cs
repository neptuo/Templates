using Neptuo.ComponentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomGeneratorContext : DefaultCodeGeneratorContext, INamingContext
    {
        public INaming Naming { get; set; }

        public CodeDomGeneratorContext(INaming naming, TextWriter output, ICodeGeneratorService codeGeneratorService, IDependencyProvider dependencyProvider, ICollection<IErrorInfo> errors = null)
            : base(output, codeGeneratorService, dependencyProvider, errors)
        {
            Naming = naming;
        }
    }
}
