using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;

namespace Neptuo.Web.Framework.Compilation
{
    public class ContentGeneratorContext : GeneratorContext
    {
        public CodeGeneratorService GeneratorService { get; set; }

        public GeneratorContext GeneratorContext { get; set; }

        public ContentGeneratorContext(GeneratorContext compilerContext, CodeGeneratorService compilerService)
        {
            GeneratorContext = compilerContext;
            GeneratorService = compilerService;
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
