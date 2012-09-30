using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public class XmlBuilderContext : ContentGeneratorContext
    {
        public IContentCodeGenerator ContentGenerator { get; set; }

        public XmlBuilderContext(ContentGeneratorContext baseContext, IContentCodeGenerator contentGenerator)
            : base(baseContext.GeneratorContext, baseContext.GeneratorService)
        {
            CodeGenerator = baseContext.CodeGenerator;
            GeneratorContext = baseContext.GeneratorContext;
            ServiceProvider = baseContext.ServiceProvider;
            ParentInfo = baseContext.ParentInfo;
            ContentGenerator = contentGenerator;
        }
    }
}
