using Neptuo.Web.Framework.Compilation.CodeGenerators;
using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Compilation.Parsers;
using Neptuo.Web.Framework.Utils;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public class CodeDomViewService : IViewService
    {
        public IParserService ParserService { get; private set; }
        public ICodeGeneratorService CodeGeneratorService { get; private set; }
        public IServiceProvider ServiceProvider { get; set; }

        public string BinDirectory { get; set; }
        public string TempDirectory { get; set; }
        public bool DebugMode { get; set; }

        public CodeDomViewService(IServiceProvider serviceProvider)
        {
            ParserService = new DefaultParserService();
            CodeGeneratorService = new DefaultCodeGeneratorService();
            ServiceProvider = serviceProvider;
        }

        public IGeneratedView Process(string fileName)
        {
            if (!File.Exists(fileName))
                throw new CodeDomViewServiceException("View doesn't exist!");

            string assemblyName = GetAssemblyName(fileName);
            if (!IsCompiled(assemblyName))
                CompileView(assemblyName, File.ReadAllText(fileName));

            return CreateGeneratedView(assemblyName);
        }

        private IGeneratedView CreateGeneratedView(string assemblyName)
        {
            Assembly views = Assembly.LoadFile(assemblyName);
            Type generatedView = views.GetType(
                String.Format("{0}.{1}", CodeDomGenerator.Names.CodeNamespace, CodeDomGenerator.Names.ClassName)
            );

            IGeneratedView view = (IGeneratedView)Activator.CreateInstance(generatedView);
            return view;
        }

        private void CompileView(string assemblyName, string viewContent)
        {
            string sourceCode = GenerateCodeFromView(viewContent);

            if (DebugMode)
                File.WriteAllText("GeneratedView.cs", sourceCode);//TODO: Do it better!

            CodeDomCompiler compiler = new CodeDomCompiler();
            compiler.IncludeDebugInformation = DebugMode;
            compiler.AddReferencedFolder(BinDirectory);

            CompilerResults cr = compiler.CompileAssemblyFromSource(sourceCode, assemblyName);
            if (cr.Errors.Count > 0)
                throw new CodeDomViewServiceException("Error compiling view!");
        }

        private string GenerateCodeFromView(string viewContent)
        {
            IPropertyDescriptor contentProperty = new ListAddPropertyDescriptor(typeof(BaseViewPage).GetProperty(TypeHelper.PropertyName<BaseViewPage>(v => v.Content)));
            bool parserResult = ParserService.ProcessContent(viewContent, new DefaultParserServiceContext(ServiceProvider, contentProperty));
            if (!parserResult)
                throw new CodeDomViewServiceException("Error parsing view content!");

            TextWriter writer = new StringWriter();
            bool generatorResult = CodeGeneratorService.GeneratedCode("CSharp", contentProperty, new DefaultCodeGeneratorContext(writer));
            if (!generatorResult)
                throw new CodeDomViewServiceException("Error generating code from view!");

            return writer.ToString();
        }

        private bool IsCompiled(string assemblyName)
        {
            return !DebugMode && File.Exists(assemblyName);
        }

        private string GetAssemblyName(string fileName)
        {
            return Path.Combine(TempDirectory, String.Format("View_{0}.dll", HashHelper.Sha1(File.ReadAllText(fileName))));
        }
    }

    public class CodeDomViewServiceException : Exception
    {
        public CodeDomViewServiceException(string message)
            : base(message)
        { }
    }

    static class HashHelper
    {
        public static string Sha1(string text)
        {
            SHA1 hasher = SHA1.Create();
            byte[] hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(text));
            StringBuilder result = new StringBuilder();
            foreach (byte hashPart in hash)
                result.Append(hashPart.ToString("X2"));

            return result.ToString();
        }
    }
}
