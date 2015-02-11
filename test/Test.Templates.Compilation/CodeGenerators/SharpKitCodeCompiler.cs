using Neptuo.SharpKit.CodeGenerator;
using Neptuo.Templates.Compilation.CodeCompilers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.Compilation.CodeGenerators
{
    public class SharpKitCodeCompiler : ICodeCompiler
    {
        public object Compile(TextReader sourceCode, ICodeCompilerContext context)
        {
            string replaceClassDefinition = "[SharpKit.JavaScript.JsType(SharpKit.JavaScript.JsMode.Clr)] public sealed class ";
            string replacedSourceCode = sourceCode.ReadToEnd().Replace("public sealed class ", replaceClassDefinition);

            StringWriter output = new StringWriter();
            StringReader input = new StringReader(replacedSourceCode);

            SharpKitCompiler sharpKitGenerator = new SharpKitCompiler();
            sharpKitGenerator.AddReference("mscorlib.dll");
            sharpKitGenerator.AddReference("SharpKit.JavaScript.dll", "SharpKit.Html.dll", "SharpKit.jQuery.dll");
            //sharpKitGenerator.AddPlugin("Neptuo.SharpKit.Exugin", "Neptuo.SharpKit.Exugin.ExuginPlugin");

            sharpKitGenerator.AddReferenceFolder(Environment.CurrentDirectory);

            sharpKitGenerator.RemoveReference("System.Web.dll");

            sharpKitGenerator.TempDirectory = Environment.CurrentDirectory;

            try
            {
                sharpKitGenerator.Generate(new SharpKitCompilerContext(input, output));
                return output.ToString();
            }
            catch (SharpKitCompilerException e)
            {
                Console.WriteLine("Error.");

                foreach (string line in e.ExecuteResult.Output)
                    Console.WriteLine(line);
            }

            return null;
        }
    }
}
