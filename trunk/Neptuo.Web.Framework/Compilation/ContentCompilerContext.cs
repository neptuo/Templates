using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Parser;

namespace Neptuo.Web.Framework.Compilation
{
    public class ContentCompilerContext
    {
        public CodeGenerator CodeGenerator { get; set; }

        public CompilerContext CompilerContext { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public IContentParser Parser { get; set; }
    }
}
