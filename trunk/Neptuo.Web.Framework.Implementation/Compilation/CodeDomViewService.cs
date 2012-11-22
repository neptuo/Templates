using Neptuo.Web.Framework.Compilation.CodeGenerators;
using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Compilation.Parsers;
using Neptuo.Web.Framework.Compilation.PreProcessing;
using Neptuo.Web.Framework.Configuration;
using Neptuo.Web.Framework.Utils;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
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
        public IPreProcessorService PreProcessorService { get; private set; }
        public CodeDomGenerator CodeDomGenerator { get; private set; }
        protected ICodeGeneratorService CodeGeneratorService { get; private set; }
        public IFileProvider FileProvider { get; private set; }

        public ICollection<string> BinDirectories { get; set; }
        public string TempDirectory { get; set; }
        public bool DebugMode { get; set; }

        public CodeDomViewService()
        {
            ParserService = new DefaultParserService();
            PreProcessorService = new DefaultPreProcessorService();
            CodeGeneratorService = new DefaultCodeGeneratorService();
            CodeDomGenerator = new CodeDomGenerator();
            CodeGeneratorService.AddGenerator("CSharp", CodeDomGenerator);

            BinDirectories = new List<string>();
        }

        public IGeneratedView Process(string fileName, IViewServiceContext context)
        {
            if (FileProvider == null)
                FileProvider = context.DependencyProvider.Resolve<IFileProvider>();

            if (!FileProvider.Exists(fileName))
                throw new CodeDomViewServiceException("View doesn't exist!");

            string assemblyName = GetAssemblyName(fileName);
            if (!IsCompiled(assemblyName))
                CompileView(assemblyName, FileProvider.GetFileContent(fileName), context);

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

        private void CompileView(string assemblyName, string viewContent, IViewServiceContext context)
        {
            string sourceCode = GenerateCodeFromView(viewContent, context);

            if (DebugMode)
                File.WriteAllText(Path.Combine(TempDirectory, "GeneratedView.cs"), sourceCode);//TODO: Do it better!

            CodeDomCompiler compiler = new CodeDomCompiler();
            compiler.IncludeDebugInformation = DebugMode;

            foreach (string directory in BinDirectories)
                compiler.AddReferencedFolder(directory);

            CompilerResults cr = compiler.CompileAssemblyFromSource(sourceCode, assemblyName);
            if (cr.Errors.Count > 0)
            {
                ICollection<IErrorInfo> errors = new List<IErrorInfo>();
                foreach (CompilerError error in cr.Errors)
                    errors.Add(new ErrorInfo(error.Line, error.Column, error.ErrorText));

                throw new CodeDomViewServiceException("Error compiling view!", errors);
            }
        }

        private string GenerateCodeFromView(string viewContent, IViewServiceContext context)
        {
            ICollection<IErrorInfo> errors = new List<IErrorInfo>();
            IPropertyDescriptor contentProperty = new ListAddPropertyDescriptor(typeof(BaseViewPage).GetProperty(TypeHelper.PropertyName<BaseViewPage>(v => v.Content)));
            bool parserResult = ParserService.ProcessContent(viewContent, new DefaultParserServiceContext(context.DependencyProvider, contentProperty, errors));
            if (!parserResult)
                throw new CodeDomViewServiceException("Error parsing view content!", errors);

            PreProcessorService.Process(contentProperty, new DefaultPreProcessorServiceContext(context.DependencyProvider));

            TextWriter writer = new StringWriter();
            bool generatorResult = CodeGeneratorService.GeneratedCode("CSharp", contentProperty, new DefaultCodeGeneratorServiceContext(writer, context.DependencyProvider, errors));
            if (!generatorResult)
                throw new CodeDomViewServiceException("Error generating code from view!", errors);

            return writer.ToString();
        }

        private bool IsCompiled(string assemblyName)
        {
            return !DebugMode && File.Exists(assemblyName);
        }

        private string GetAssemblyName(string fileName)
        {
            return Path.Combine(TempDirectory, String.Format("View_{0}.dll", HashHelper.Sha1(FileProvider.GetFileContent(fileName))));
        }
    }
}
