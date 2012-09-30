using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public class ValueGeneratorContext : GeneratorContext
    {
        public CodeGeneratorService GeneratorService { get; set; }

        public GeneratorContext GeneratorContext { get; set; }

        public ValueGeneratorContext(GeneratorContext generatorContext, CodeGeneratorService generatorService)
        {
            GeneratorContext = generatorContext;
            GeneratorService = generatorService;
            CodeGenerator = generatorContext.CodeGenerator;
            ServiceProvider = generatorContext.ServiceProvider;
            ParentInfo = generatorContext.ParentInfo;
        }
    }
}
