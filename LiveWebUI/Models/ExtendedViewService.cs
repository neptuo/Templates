using Neptuo.Templates.Compilation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiveWebUI.Models
{
    public class ExtendedViewService : CodeDomViewService
    {
        public string GenerateSourceCode(string viewContent, IViewServiceContext context, INaming naming)
        {
            string sourceCode = GenerateSourceCodeFromView(viewContent, context, naming);
            return sourceCode;
        }

        public void CompileViewContent(string viewContent, IViewServiceContext context, INaming naming)
        {
            base.CompileView(viewContent, context, naming);
        }
    }
}