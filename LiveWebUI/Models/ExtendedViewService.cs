using Neptuo.SharpKit.CodeGenerator;
using Neptuo.Templates.Compilation;
using System;
using System.Collections.Generic;
using System.IO;
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

        public string GenerateJavascript(string viewContent, IViewServiceContext context, INaming naming)
        {
            string sourceCode = GenerateSourceCode(viewContent, context, naming);
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