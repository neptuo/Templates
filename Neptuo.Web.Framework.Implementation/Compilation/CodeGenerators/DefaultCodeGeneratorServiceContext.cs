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

        public IServiceProvider ServiceProvider { get; set; }

        public DefaultCodeGeneratorServiceContext(TextWriter output, IServiceProvider serviceProvider)
        {
            Output = output;
            ServiceProvider = serviceProvider;
        }
    }
}
