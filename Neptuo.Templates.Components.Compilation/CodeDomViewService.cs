using Neptuo.Linq.Expressions;
using Neptuo.Security.Cryptography;
using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Templates.Compilation.PreProcessing;
//using Neptuo.Templates.Configuration;
//using Neptuo.Templates.Utils;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Neptuo.Templates.Compilation
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

            return ProcessContent(FileProvider.GetFileContent(fileName), context);
        }

        public IGeneratedView ProcessContent(string viewContent, IViewServiceContext context)
        {
            string assemblyName = GetAssemblyPathForContent(viewContent);
            if (!AssemblyExists(assemblyName))
                CompileView(assemblyName, viewContent, context);

            return CreateGeneratedView(assemblyName);
        }

        protected virtual IGeneratedView CreateGeneratedView(string assemblyName)
        {
            Assembly views = Assembly.LoadFile(assemblyName);
            Type generatedView = views.GetType(
                String.Format("{0}.{1}", CodeDomGenerator.Names.CodeNamespace, CodeDomGenerator.Names.ClassName)
            );

            IGeneratedView view = (IGeneratedView)Activator.CreateInstance(generatedView);
            return view;
        }

        protected virtual void CompileView(string assemblyName, string viewContent, IViewServiceContext context)
        {
            string sourceCode = GenerateSourceCodeFromView(viewContent, context);

            if (DebugMode)
                File.WriteAllText(Path.Combine(TempDirectory, "GeneratedView.cs"), sourceCode);//TODO: Do it better!

            DebugUtils.Run("Compile", () =>
            {
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
            });
        }

        protected virtual string GenerateSourceCodeFromView(string viewContent, IViewServiceContext context)
        {
            ICollection<IErrorInfo> errors = new List<IErrorInfo>();
            IPropertyDescriptor contentProperty = ParseViewContent(viewContent, context);

            PreProcessorService.Process(contentProperty, new DefaultPreProcessorServiceContext(context.DependencyProvider));

            return GenerateSourceCode(contentProperty, context);
        }

        protected virtual IPropertyDescriptor GetRootPropertyDescriptor(IViewServiceContext context)
        {
            IPropertyInfo propertyInfo = new TypePropertyInfo(typeof(BaseViewPage).GetProperty(TypeHelper.PropertyName<BaseViewPage>(v => v.Content)));
            return new ListAddPropertyDescriptor(propertyInfo);
        }

        protected virtual IPropertyDescriptor ParseViewContent(string viewContent, IViewServiceContext context)
        {
            ICollection<IErrorInfo> errors = new List<IErrorInfo>();
            IPropertyDescriptor contentProperty = GetRootPropertyDescriptor(context);

            DebugUtils.Run("ParseContent", () =>
            {
                bool parserResult = ParserService.ProcessContent(viewContent, new DefaultParserServiceContext(context.DependencyProvider, contentProperty, errors));

                if (!parserResult)
                    throw new CodeDomViewServiceException("Error parsing view content!", errors);
            });

            return contentProperty;
        }

        protected virtual string GenerateSourceCode(IPropertyDescriptor contentProperty, IViewServiceContext context)
        {
            ICollection<IErrorInfo> errors = new List<IErrorInfo>();
            TextWriter writer = new StringWriter();

            DebugUtils.Run("GenerateSourceCode", () =>
            {
                bool generatorResult = CodeGeneratorService.GeneratedCode("CSharp", contentProperty, new DefaultCodeGeneratorServiceContext(writer, context.DependencyProvider, errors));
                if (!generatorResult)
                    throw new CodeDomViewServiceException("Error generating code from view!", errors);
            });

            return writer.ToString();
        }

        /// <summary>
        /// Returns whether assembly file exists.
        /// </summary>
        /// <param name="assemblyName">Assembly name.</param>
        /// <returns>True if assembly exists.</returns>
        protected virtual bool AssemblyExists(string assemblyName)
        {
            return !DebugMode && File.Exists(assemblyName);//TODO: Should use FileProvider!
        }

        /// <summary>
        /// Returns full path for assembly.
        /// </summary>
        /// <param name="fileName">View filename.</param>
        /// <returns>Full path for assembly.</returns>
        protected virtual string GetAssemblyPath(string fileName)
        {
            return GetAssemblyPathForContent(FileProvider.GetFileContent(fileName));
        }

        /// <summary>
        /// Returns full path for assembly.
        /// </summary>
        /// <param name="viewContent">Content of view.</param>
        /// <returns>Full path for assembly.</returns>
        protected virtual string GetAssemblyPathForContent(string viewContent)
        {
            return Path.Combine(TempDirectory, String.Format("View_{0}.dll", HashHelper.Sha1(viewContent)));
        }
    }
}
