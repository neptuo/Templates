using Neptuo.Web.Framework.Compilation.CodeGenerators;
using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Compilation.Parsers;
using Neptuo.Web.Framework.Compilation.PreProcessing;
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
        public IPreProcessorService PreProcessorService { get; private set; }
        public ICodeGeneratorService CodeGeneratorService { get; private set; }
        public IDependencyProvider DependencyProvider { get; set; }

        public string BinDirectory { get; set; }
        public string TempDirectory { get; set; }
        public bool DebugMode { get; set; }

        public CodeDomViewService(IDependencyProvider dependencyProvider)
        {
            ParserService = new DefaultParserService();
            PreProcessorService = new DefaultPreProcessorService();
            CodeGeneratorService = new DefaultCodeGeneratorService();
            DependencyProvider = dependencyProvider;
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
                File.WriteAllText(Path.Combine(TempDirectory, "GeneratedView.cs"), sourceCode);//TODO: Do it better!

            CodeDomCompiler compiler = new CodeDomCompiler();
            compiler.IncludeDebugInformation = DebugMode;
            compiler.AddReferencedFolder(BinDirectory);

            CompilerResults cr = compiler.CompileAssemblyFromSource(sourceCode, assemblyName);
            if (cr.Errors.Count > 0)
            {
                ICollection<IErrorInfo> errors = new List<IErrorInfo>();
                foreach (CompilerError error in cr.Errors)
                    errors.Add(new ErrorInfo(error.Line, error.Column, error.ErrorText));

                throw new CodeDomViewServiceException("Error compiling view!", errors);
            }
        }

        private string GenerateCodeFromView(string viewContent)
        {
            ICollection<IErrorInfo> errors = new List<IErrorInfo>();
            IPropertyDescriptor contentProperty = new ListAddPropertyDescriptor(typeof(BaseViewPage).GetProperty(TypeHelper.PropertyName<BaseViewPage>(v => v.Content)));
            bool parserResult = ParserService.ProcessContent(viewContent, new DefaultParserServiceContext(DependencyProvider, contentProperty, errors));
            if (!parserResult)
                throw new CodeDomViewServiceException("Error parsing view content!", errors);

            PreProcessorService.Process(contentProperty, new DefaultPreProcessorServiceContext(DependencyProvider));

            TextWriter writer = new StringWriter();
            bool generatorResult = CodeGeneratorService.GeneratedCode("CSharp", contentProperty, new DefaultCodeGeneratorServiceContext(writer, DependencyProvider, errors));
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
            return Path.Combine(TempDirectory, String.Format("View_{0}.dll", HashHelper.Sha1(File.ReadAllText(fileName))));
        }
    }
}
