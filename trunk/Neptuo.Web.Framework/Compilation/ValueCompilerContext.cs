using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public class ValueCompilerContext : CompilerContext
    {
        public CompilerService CompilerService { get; set; }

        public CompilerContext CompilerContext { get; set; }

        public ValueCompilerContext(CompilerContext compilerContext, CompilerService compilerService)
        {
            CompilerContext = compilerContext;
            CompilerService = compilerService;
            CodeGenerator = compilerContext.CodeGenerator;
            ServiceProvider = compilerContext.ServiceProvider;
            ParentInfo = compilerContext.ParentInfo;
        }
    }
}
