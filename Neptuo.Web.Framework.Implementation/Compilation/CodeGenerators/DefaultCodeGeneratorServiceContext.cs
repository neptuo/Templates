using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators
{
    public class DefaultCodeGeneratorServiceContext : ICodeGeneratorServiceContext
    {
        public TextWriter Output { get; set; }
        public IDependencyProvider DependencyProvider { get; set; }

        public DefaultCodeGeneratorServiceContext(TextWriter output, IDependencyProvider dependencyProvider)
        {
            Output = output;
            DependencyProvider = dependencyProvider;
        }
    }
}
