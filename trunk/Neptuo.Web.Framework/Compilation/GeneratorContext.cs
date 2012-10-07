using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public class GeneratorContext
    {
        public CodeGenerator CodeGenerator { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public ParentInfo ParentInfo { get; set; }
    }
}
