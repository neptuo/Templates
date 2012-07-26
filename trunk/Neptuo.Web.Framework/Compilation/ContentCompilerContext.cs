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
        public CodeMemberField Parent { get; set; }

        public string PropertyName { get; set; }

        public string MethodName { get; set; }

        public Type RequiredType { get; set; }

        public ParentInfo(CodeMemberField parent, string propertyName, string methodName, Type requiredType)
        {
            Parent = parent;
            PropertyName = propertyName;
            MethodName = methodName;
            RequiredType = requiredType;
        }
    }
}
