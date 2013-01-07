using Neptuo.Web.Framework;
using Neptuo.Web.Framework.Compilation;
using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Utils;
using System;
using System.IO;

namespace DemoWebUI.Models
{
    public class ExtendedCodeDomViewService : CodeDomViewService
    {
        public event EventHandler<SourceCodeEventArgs> OnSourceCodeGenerated;

        protected override string GenerateSourceCode(IPropertyDescriptor contentProperty, IViewServiceContext context)
        {
            string sourceCode = base.GenerateSourceCode(contentProperty, context);

            if (OnSourceCodeGenerated != null)
                OnSourceCodeGenerated(this, new SourceCodeEventArgs(sourceCode));

            return sourceCode;
        }

        public IGeneratedView ProcessContent(string viewContent, IViewServiceContext context)
        {
            string fileName = Guid.NewGuid().ToString();
            string assemblyName = Path.Combine(TempDirectory, String.Format("Live_{0}.dll", HashHelper.Sha1(viewContent)));
            if (!AssemblyExists(assemblyName))
                CompileView(assemblyName, viewContent, context);

            return CreateGeneratedView(assemblyName);
        }
    }

    public class SourceCodeEventArgs : EventArgs
    {
        public string SourceCode { get; private set; }

        public SourceCodeEventArgs(string sourceCode)
        {
            SourceCode = sourceCode;
        }
    }
}