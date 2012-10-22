using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public class DefaultCodeGeneratorContext : ICodeGeneratorContext
    {
        public TextWriter Output { get; set; }

        public DefaultCodeGeneratorContext(TextWriter output)
        {
            Output = output;
        }
    }
}
