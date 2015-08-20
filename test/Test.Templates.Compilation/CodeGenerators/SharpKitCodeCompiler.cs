using Neptuo.Compilers;
using Neptuo.Compilers.Errors;
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

            CompilerFactory factory = new CompilerFactory();
            ISharpKitCompiler compiler = factory.CreateSharpKit();
            compiler.References().AddAssembly("mscorlib.dll");
            compiler.References().AddAssembly("SharpKit.JavaScript.dll");
            compiler.References().AddAssembly("SharpKit.Html.dll");
            compiler.References().AddAssembly("SharpKit.jQuery.dll");
            //compiler.AddPlugin("Neptuo.SharpKit.Exugin", "Neptuo.SharpKit.Exugin.ExuginPlugin");
            compiler.References().AddDirectory(Environment.CurrentDirectory);
            //compiler.References().RemoveReference("System.Web.dll");

            compiler.AddTempDirectory(Environment.CurrentDirectory);

            string filePath = Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString() + ".js");


            ICompilerResult result = compiler.FromSourceCode(replacedSourceCode, filePath);
            if (result.IsSuccess)
            {
                string content = File.ReadAllText(filePath);
                File.Delete(filePath);
                return content;
            }
            else
            {
                foreach (IErrorInfo error in result.Errors)
                    Console.WriteLine("{0}:{1} -> {2}", error.LineNumber, error.ColumnIndex, error.ErrorText);

                return null;
            }
        }
    }
}
