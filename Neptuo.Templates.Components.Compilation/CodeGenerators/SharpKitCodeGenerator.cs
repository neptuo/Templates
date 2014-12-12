using Neptuo.SharpKit.CodeGenerator;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// SharpKit javascrip generator.
    /// </summary>
    public class SharpKitCodeGenerator : ICodeGenerator
    {
        protected ICodeGenerator InnerGenerator { get; private set; }
        public ICollection<string> BinDirectories { get; private set; }
        public string TempDirectory { get; set; }
        public bool RunCsc { get; set; }

        public SharpKitCodeGenerator(ICodeGenerator innerGenerator)
        {
            InnerGenerator = innerGenerator;
            BinDirectories = new List<string>();
            RunCsc = true;
        }

        public bool ProcessTree(ICodeObject codeObject, ICodeGeneratorContext context)
        {
            StringWriter innerOutput = new StringWriter();
            ICodeGeneratorContext innerContext = new DefaultCodeGeneratorContext(
                innerOutput,
                context.CodeGeneratorService,
                context.DependencyProvider,
                context.Errors
            );

            if (!InnerGenerator.ProcessTree(codeObject, innerContext))
                return false;

            return GenerateJavascriptFromCSharpCode(innerOutput, context);
        }

        protected virtual bool GenerateJavascriptFromCSharpCode(StringWriter innerOutput, ICodeGeneratorContext context)
        {
            string replaceClassDefinition = "[SharpKit.JavaScript.JsType(SharpKit.JavaScript.JsMode.Clr)] public sealed class ";
            string sourceCode = innerOutput.ToString().Replace("public sealed class ", replaceClassDefinition);

            StringWriter output = new StringWriter();
            StringReader input = new StringReader(sourceCode);

            SharpKitCompiler sharpKitGenerator = new SharpKitCompiler();
            sharpKitGenerator.RunCsc = RunCsc;
            sharpKitGenerator.AddReference("mscorlib.dll");
            sharpKitGenerator.AddReference("SharpKit.JavaScript.dll", "SharpKit.Html.dll", "SharpKit.jQuery.dll");
            sharpKitGenerator.AddPlugin("Neptuo.SharpKit.Exugin", "Neptuo.SharpKit.Exugin.ExuginPlugin");

            foreach (string folder in BinDirectories)
                sharpKitGenerator.AddReferenceFolder(folder);

            sharpKitGenerator.RemoveReference("System.Web.dll");
            //sharpKitGenerator.RemoveReference("SharpKit.JavaScript.dll");

            sharpKitGenerator.TempDirectory = TempDirectory;
            sharpKitGenerator.Generate(new SharpKitCompilerContext(input, output));
            context.Output.Write(output);

            return true;
        }
    }
}
