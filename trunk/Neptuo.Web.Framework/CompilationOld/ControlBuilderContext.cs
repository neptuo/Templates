using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Parser.HtmlContent;

namespace Neptuo.Web.Framework.CompilationOld
{
    public class ControlBuilderContext : ContentCompilerContext
    {
        public IContentCompiler<HtmlTag> ContentCompiler { get; set; }

        public ControlBuilderContext(ContentCompilerContext baseContext, IContentCompiler<HtmlTag> contentCompiler)
        {
            CodeGenerator = baseContext.CodeGenerator;
            CompilerContext = baseContext.CompilerContext;
            ServiceProvider = baseContext.ServiceProvider;
            Parser = baseContext.Parser;
            ParentInfo = baseContext.ParentInfo;
            ContentCompiler = contentCompiler;
        }
    }
}
