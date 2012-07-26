using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public class XmlBuilderContext : ContentCompilerContext
    {
        public IContentCompiler ContentCompiler { get; set; }

        public XmlBuilderContext(ContentCompilerContext baseContext, IContentCompiler contentCompiler)
            : base(baseContext.CompilerContext, baseContext.CompilerService)
        {
            CodeGenerator = baseContext.CodeGenerator;
            CompilerContext = baseContext.CompilerContext;
            ServiceProvider = baseContext.ServiceProvider;
            ParentInfo = baseContext.ParentInfo;
            ContentCompiler = contentCompiler;
        }
    }
}
