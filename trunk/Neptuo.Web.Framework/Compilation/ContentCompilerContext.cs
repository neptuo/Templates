using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;

namespace Neptuo.Web.Framework.Compilation
{
    public class ContentCompilerContext : CompilerContext
    {
        public CompilerService CompilerService { get; set; }

        public CompilerContext CompilerContext { get; set; }

        public ContentCompilerContext(CompilerContext compilerContext, CompilerService compilerService)
        {
            CompilerContext = compilerContext;
            CompilerService = compilerService;
            CodeGenerator = compilerContext.CodeGenerator;
            ServiceProvider = compilerContext.ServiceProvider;
            ParentInfo = compilerContext.ParentInfo;
        }
    }

    public class ParentInfo
    {
        public CodeObjectCreator Creator { get; set; }

        public string PropertyName { get; set; }

        public string MethodName { get; set; }

        public Type RequiredType { get; set; }

        public bool AsReturnStatement { get; set; }

        public ParentInfo(CodeObjectCreator creator, string propertyName, string methodName, Type requiredType)
        {
            Creator = creator;
            PropertyName = propertyName;
            MethodName = methodName;
            RequiredType = requiredType;
        }
    }
}
