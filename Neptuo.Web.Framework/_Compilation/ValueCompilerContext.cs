using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public class ValueGeneratorContext : XGeneratorContext
    {
        public CodeGeneratorService GeneratorService { get; set; }

        public XGeneratorContext GeneratorContext { get; set; }

        public ValueGeneratorContext(XGeneratorContext generatorContext, CodeGeneratorService generatorService)
        {
            GeneratorContext = generatorContext;
            GeneratorService = generatorService;
            CodeGenerator = generatorContext.CodeGenerator;
            ServiceProvider = generatorContext.ServiceProvider;
            ParentInfo = generatorContext.ParentInfo;
        }
    }
}
