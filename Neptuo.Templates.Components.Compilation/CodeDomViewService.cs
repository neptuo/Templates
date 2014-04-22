using Neptuo.Linq.Expressions;
using Neptuo.Security.Cryptography;
using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Templates.Compilation.PreProcessing;
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
        private bool useDefaultGenerators;
        private string tempDirectory;

        private CodeDomGenerator codeDomGenerator;
        private SharpKitCodeGenerator javascriptGenerator;

        public CodeDomDebugMode DebugMode { get; set; }

        public IParserService ParserService { get; private set; }
        public IPreProcessorService PreProcessorService { get; private set; }
        public ICodeGeneratorService CodeGeneratorService { get; private set; }
        public IFileProvider FileProvider { get; private set; }
        public INamingService NamingService { get; set; }
        public ICollection<string> BinDirectories { get; set; }

        public CodeDomGenerator CodeDomGenerator
        {
            get
            {
                if (!useDefaultGenerators)
                    throw new InvalidOperationException("This CodeDomViewService is configured without default CodeDomGenerator.");

                return codeDomGenerator;
            }
        }

        public SharpKitCodeGenerator JavascriptGenerator
        {
            get
            {
                if (!useDefaultGenerators)
                    throw new InvalidOperationException("This CodeDomViewService is configured without default JavascriptGenerator.");

                return javascriptGenerator;
            }
        }

        public string TempDirectory
        {
            get { return tempDirectory; }
            set
            {
                tempDirectory = value;
                if (useDefaultGenerators)
                    JavascriptGenerator.TempDirectory = value;
            }
        }

        /// <summary>
        /// Creates instance as main view service.
        /// </summary>
        /// <param name="useDefaultGenerators">Whether create </param>
        public CodeDomViewService(bool useDefaultGenerators)
        {
            this.useDefaultGenerators = useDefaultGenerators;

            ParserService = new DefaultParserService();
            PreProcessorService = new DefaultPreProcessorService();
            CodeGeneratorService = new DefaultCodeGeneratorService();

            if (useDefaultGenerators)
            {
                codeDomGenerator = new CodeDomGenerator();
                javascriptGenerator = new SharpKitCodeGenerator(CodeDomGenerator);
                CodeGeneratorService.AddGenerator("CSharp", CodeDomGenerator);
                CodeGeneratorService.AddGenerator("Javascript", JavascriptGenerator);

                BinDirectories = new NotifyList<string>(
                    JavascriptGenerator.BinDirectories.Add,
                    JavascriptGenerator.BinDirectories.Remove,
                    JavascriptGenerator.BinDirectories.Clear
                );
            }
            else
            {
                BinDirectories = new NotifyList<string>();
            }
        }

        public object Process(string fileName, IViewServiceContext context)
        {
            EnsureFileProvider(context);
            EnsureNamingService(context);

            if (!FileProvider.Exists(fileName))
                throw new CodeDomViewServiceException(String.Format("View '{0}' doesn't exist!", fileName));

            return ProcessContent(FileProvider.GetFileContent(fileName), context, NamingService.FromFile(fileName));
        }

        public object ProcessContent(string viewContent, IViewServiceContext context)
        {
            EnsureNamingService(context);
            return ProcessContent(viewContent, context, NamingService.FromContent(viewContent));
        }

        protected virtual object ProcessContent(string viewContent, IViewServiceContext context, INaming naming)
        {
            EnsureFileProvider(context);
            string assemblyPath = GetAssemblyPath(naming);
            if (!AssemblyExists(assemblyPath))
                CompileView(viewContent, context, naming);

            return CreateGeneratedView(naming);
        }

        protected void EnsureNamingService(IViewServiceContext context)
        {
            if (NamingService == null)
                NamingService = context.DependencyProvider.Resolve<INamingService>();
        }

        protected void EnsureFileProvider(IViewServiceContext context)
        {
            if (FileProvider == null)
                FileProvider = context.DependencyProvider.Resolve<IFileProvider>();
        }

        protected virtual object CreateGeneratedView(INaming naming)
        {
            Assembly views = Assembly.LoadFile(GetAssemblyPath(naming));
            Type generatedView = views.GetType(
                String.Format("{0}.{1}", naming.ClassNamespace, naming.ClassName)
            );

            object view = Activator.CreateInstance(generatedView);
            return view;
        }

        protected virtual void CompileView(string viewContent, IViewServiceContext context, INaming naming)
        {
            string sourceCode = GenerateSourceCodeFromView(viewContent, context, naming);

            if (DebugMode.HasFlag(CodeDomDebugMode.GenerateSourceCode))
                File.WriteAllText(Path.Combine(TempDirectory, naming.AssemblyName.Replace(".dll", ".cs")), sourceCode);//TODO: Do it better!

            DebugUtils.Run("Compile", () =>
            {
                CodeDomCompiler compiler = new CodeDomCompiler();
                compiler.IncludeDebugInformation = DebugMode.HasFlag(CodeDomDebugMode.GeneratePdb);

                foreach (string directory in BinDirectories)
                    compiler.AddReferencedFolder(directory);

                CompilerResults cr = compiler.CompileAssemblyFromSource(sourceCode, GetAssemblyPath(naming));
                if (cr.Errors.Count > 0)
                {
                    ICollection<IErrorInfo> errors = new List<IErrorInfo>();
                    foreach (CompilerError error in cr.Errors)
                        errors.Add(new ErrorInfo(error.Line, error.Column, error.ErrorText));

                    throw new CodeDomViewServiceException("Error compiling view!", errors, viewContent, sourceCode);
                }
            });
        }

        protected virtual string GenerateSourceCodeFromView(string viewContent, IViewServiceContext context, INaming naming)
        {
            ICollection<IErrorInfo> errors = new List<IErrorInfo>();
            IPropertyDescriptor contentProperty = ParseViewContent(viewContent, context);

            PreProcessorService.Process(contentProperty, new DefaultPreProcessorServiceContext(context.DependencyProvider));

            return GenerateSourceCode(contentProperty, context, naming);
        }

        protected virtual IPropertyDescriptor GetRootPropertyDescriptor(IViewServiceContext context)
        {
            IPropertyInfo propertyInfo = new TypePropertyInfo(typeof(BaseGeneratedView).GetProperty(TypeHelper.PropertyName<BaseGeneratedView>(v => v.Content)));
            return new ListAddPropertyDescriptor(propertyInfo);
        }

        protected virtual IPropertyDescriptor ParseViewContent(string viewContent, IViewServiceContext context, IParserService parserService = null)
        {
            ICollection<IErrorInfo> errors = new List<IErrorInfo>();
            IPropertyDescriptor contentProperty = GetRootPropertyDescriptor(context);

            DebugUtils.Run("ParseContent", () =>
            {
                bool parserResult = (parserService ?? ParserService).ProcessContent(viewContent, new DefaultParserServiceContext(context.DependencyProvider, contentProperty, errors));

                if (!parserResult)
                    throw new CodeDomViewServiceException("Error parsing view content!", errors, viewContent);
            });

            return contentProperty;
        }

        protected virtual string GenerateSourceCode(IPropertyDescriptor contentProperty, IViewServiceContext context, INaming naming, ICodeGeneratorService codeGeneratorService = null)
        {
            ICollection<IErrorInfo> errors = new List<IErrorInfo>();
            TextWriter writer = new StringWriter();

            DebugUtils.Run("GenerateSourceCode", () =>
            {
                ICodeGeneratorServiceContext generatorContext = new CodeDomGeneratorServiceContext(naming, writer, context.DependencyProvider, errors);
                bool generatorResult = (codeGeneratorService ?? CodeGeneratorService).GeneratedCode("CSharp", contentProperty, generatorContext);
                if (!generatorResult)
                    throw new CodeDomViewServiceException("Error generating code from view!", errors);
            });

            return writer.ToString();
        }

        protected virtual string GenerateJavascriptSourceCode(IPropertyDescriptor contentProperty, IViewServiceContext context, INaming naming, ICodeGeneratorService codeGeneratorService = null)
        {
            ICollection<IErrorInfo> errors = new List<IErrorInfo>();
            TextWriter writer = new StringWriter();

            DebugUtils.Run("GenerateSourceCode", () =>
            {
                ICodeGeneratorServiceContext generatorContext = new CodeDomGeneratorServiceContext(naming, writer, context.DependencyProvider, errors);
                bool generatorResult = (codeGeneratorService ?? CodeGeneratorService).GeneratedCode("Javascript", contentProperty, generatorContext);
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
            return !DebugMode.HasFlag(CodeDomDebugMode.AlwaysReGenerate) && FileProvider.Exists(assemblyName);
        }

        /// <summary>
        /// Returns full path for assembly.
        /// </summary>
        /// <param name="fileName">View filename.</param>
        /// <returns>Full path for assembly.</returns>
        protected virtual string GetAssemblyPath(INaming naming)
        {
            return Path.Combine(TempDirectory, naming.AssemblyName);
        }
    }
}
