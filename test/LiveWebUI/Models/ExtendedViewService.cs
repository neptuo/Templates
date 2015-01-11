using Neptuo.SharpKit.CodeGenerator;
using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LiveWebUI.Models
{
    public class ExtendedViewService : CodeDomViewService
    {
        public ExtendedViewService()
            : base(true)
        { }

        public string GenerateSourceCode(ISourceContent content, IViewServiceContext context, INaming naming)
        {
            string sourceCode = GenerateSourceCodeFromView(content, context, naming);
            return sourceCode;
        }

        public void CompileViewContent(ISourceContent content, IViewServiceContext context, INaming naming)
        {
            base.CompileView(content, context, naming);
        }

        public string GenerateJavascript(ISourceContent content, IViewServiceContext context, INaming naming)
        {
            string sourceCode = GenerateSourceCode(content, context, naming);
            sourceCode = sourceCode.Replace("public sealed class ", "[SharpKit.JavaScript.JsType(SharpKit.JavaScript.JsMode.Clr)] public sealed class ");

            StringWriter output = new StringWriter();
            StringReader input = new StringReader(sourceCode);

            SharpKitCompiler sharpKitGenerator = new SharpKitCompiler();
            sharpKitGenerator.AddReference("mscorlib.dll");
            sharpKitGenerator.AddSharpKitReference("SharpKit.JavaScript.dll", "SharpKit.Html.dll", "SharpKit.jQuery.dll");
            
            foreach (string folder in BinDirectories)
                sharpKitGenerator.AddReferenceFolder(folder);

            sharpKitGenerator.TempDirectory = TempDirectory;
            sharpKitGenerator.Generate(new SharpKitCompilerContext(input, output));
            return output.ToString();
        }
    }
}