﻿using Neptuo.Compilers;
using Neptuo.ComponentModel;
using Neptuo.Diagnostics;
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
    /// <summary>
    /// Implementation of <see cref="IViewService"/> using CodeDom.
    /// </summary>
    public class CodeDomViewService : DebugBase, IViewService
    {
        /// <summary>
        /// Flag to see whether use default (<see cref="CodeDomGenerator"/> and <see cref="SharpKitCodeGenerator"/>) generators.
        /// </summary>
        private bool useDefaultGenerators;

        /// <summary>
        /// Temp directory path.
        /// </summary>
        private string tempDirectory;

        /// <summary>
        /// Default server side generator.
        /// </summary>
        private CodeDomGenerator codeDomGenerator;

        /// <summary>
        /// Default javascript generator.
        /// </summary>
        private SharpKitCodeGenerator javascriptGenerator;

        /// <summary>
        /// Service for compiling c# sources.
        /// </summary>
        private CompilerFactory compilerFactory;

        /// <summary>
        /// Current debug information, <see cref="CodeDomDebugMode"/>.
        /// </summary>
        public CodeDomDebugMode DebugMode { get; set; }

        /// <summary>
        /// Parser service used for parsing templates.
        /// </summary>
        public IParserService ParserService { get; private set; }

        /// <summary>
        /// Pre processor service used for pre processing ast.
        /// </summary>
        public IPreProcessorService PreProcessorService { get; private set; }

        /// <summary>
        /// Code generator service used for generating source code.
        /// </summary>
        public ICodeGeneratorService CodeGeneratorService { get; private set; }

        /// <summary>
        /// File provider provides access to file system.
        /// </summary>
        public IFileProvider FileProvider { get; private set; }

        /// <summary>
        /// Current naming service used for naming generated code.
        /// </summary>
        public INamingService NamingService { get; set; }

        /// <summary>
        /// List of bin directories included in compilation process.
        /// </summary>
        public ICollection<string> BinDirectories { get; set; }

        /// <summary>
        /// Default server side generator.
        /// </summary>
        public CodeDomGenerator CodeDomGenerator
        {
            get
            {
                if (!useDefaultGenerators)
                    throw Guard.Exception.InvalidOperation("This CodeDomViewService is configured without default CodeDomGenerator.");

                return codeDomGenerator;
            }
        }

        /// <summary>
        /// Default javascript generator.
        /// </summary>
        public SharpKitCodeGenerator JavascriptGenerator
        {
            get
            {
                if (!useDefaultGenerators)
                    throw Guard.Exception.InvalidOperation("This CodeDomViewService is configured without default JavascriptGenerator.");

                return javascriptGenerator;
            }
        }

        /// <summary>
        /// Temp directory path.
        /// </summary>
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
        /// Text writer pro simple performance measurements.
        /// </summary>
        public DebugBase.DebugMessageWriter DebugWriter
        {
            get { return InnerWriter; }
            set
            {
                if (value != null)
                    InnerWriter = value;
            }
        }

        /// <summary>
        /// Creates instance as main view service.
        /// </summary>
        /// <param name="useDefaultGenerators">Whether create </param>
        public CodeDomViewService(bool useDefaultGenerators)
            : base(Console.Out)
        {
            this.useDefaultGenerators = useDefaultGenerators;
            this.compilerFactory = new CompilerFactory();

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

        /// <summary>
        /// Compiles <see cref="viewContent"/> and retuns <see cref="GeneratedView"/>.
        /// </summary>
        /// <param name="name">Name of required process to run.</param>
        /// <param name="viewContent">Template content.</param>
        /// <param name="context">Context information.</param>
        /// <returns><see cref="GeneratedView"/> for <paramref name="viewContent"/>.</returns>
        public object ProcessContent(string name, ISourceContent content, IViewServiceContext context)
        {
            Guard.NotNull(content, "content");
            Guard.NotNull(context, "context");

            EnsureNamingService(context);
            return ProcessContent(name, content, context, NamingService.FromContent(content.TextContent));
        }

        /// <summary>
        /// Compiles <see cref="viewContent"/> and retuns <see cref="GeneratedView"/>.
        /// </summary>
        /// <param name="name">Name of required process to run.</param>
        /// <param name="content">Template content.</param>
        /// <param name="context">Context information.</param>
        /// <param name="naming">Naming for this template compilation.</param>
        /// <returns><see cref="GeneratedView"/> for <paramref name="content"/>.</returns>
        protected virtual object ProcessContent(string name, ISourceContent content, IViewServiceContext context, INaming naming)
        {
            EnsureFileProvider(context);
            if (name == "CSharp")
            {
                string assemblyPath = GetAssemblyPath(naming);
                if (!AssemblyExists(assemblyPath))
                    CompileView(content, context, naming);

                return CreateGeneratedView(naming);
            }
            else if (name == "Javascript")
            {
                ICollection<IErrorInfo> errors = new List<IErrorInfo>();
                ICodeObject codeObject = ParseViewContent(content, context);

                PreProcessorService.Process(codeObject, new DefaultPreProcessorServiceContext(context.DependencyProvider));

                string sourceCode = GenerateJavascriptSourceCode(codeObject, context, naming);
                return sourceCode;
            }

            throw new NotSupportedException(String.Format("Not support process name '{0}'", name));
        }

        /// <summary>
        /// Ensures naming service is not null.
        /// </summary>
        /// <param name="context">Context information.</param>
        protected void EnsureNamingService(IViewServiceContext context)
        {
            if (NamingService == null)
                NamingService = context.DependencyProvider.Resolve<INamingService>();
        }

        /// <summary>
        /// Ensures file provider is not null.
        /// </summary>
        /// <param name="context">Context information.</param>
        protected void EnsureFileProvider(IViewServiceContext context)
        {
            if (FileProvider == null)
                FileProvider = context.DependencyProvider.Resolve<IFileProvider>();
        }

        /// <summary>
        /// Creates instance of generated view from <paramref name="naming"/>.
        /// </summary>
        /// <param name="naming">Naming information about view to create.</param>
        /// <returns>Instance of generated view from <paramref name="naming"/>.</returns>
        protected virtual object CreateGeneratedView(INaming naming)
        {
            Assembly views = Assembly.LoadFile(Path.Combine(TempDirectory, GetAssemblyPath(naming)));
            Type generatedView = views.GetType(
                String.Format("{0}.{1}", naming.ClassNamespace, naming.ClassName)
            );

            object view = Activator.CreateInstance(generatedView);
            return view;
        }

        /// <summary>
        /// Compiles view content into assembly.
        /// </summary>
        /// <param name="content">Template content.</param>
        /// <param name="context">Context information.</param>
        /// <param name="naming">Naming for this template compilation.</param>
        protected virtual void CompileView(ISourceContent content, IViewServiceContext context, INaming naming)
        {
            string sourceCode = GenerateSourceCodeFromView(content, context, naming);

            // If flag GenerateSourceCode, then safe it into temp
            if (DebugMode.HasFlag(CodeDomDebugMode.GenerateSourceCode))
                File.WriteAllText(Path.Combine(TempDirectory, naming.AssemblyName.Replace(".dll", ".cs")), sourceCode);//TODO: Do it better!

            DebugHelper.Debug("Compile", () =>
            {
                IStaticCompiler compiler = compilerFactory.CreateStatic();
                compiler.IsDebugMode = DebugMode.HasFlag(CodeDomDebugMode.GeneratePdb);

                foreach (string directory in BinDirectories)
                    compiler.References.AddDirectory(directory);

                ICompilerResult result = compiler.FromSourceCode(sourceCode, GetAssemblyPath(naming));
                if (!result.IsSuccess)
                    throw new CodeDomViewServiceException("Error compiling view!", result.Errors, content.TextContent, sourceCode);
            });
        }

        /// <summary>
        /// Generates C# source code from <paramref name="content"/>.
        /// </summary>
        /// <param name="content">Template content.</param>
        /// <param name="context">Context information.</param>
        /// <param name="naming">Naming for this template compilation.</param>
        /// <returns>Generated C# source code from <paramref name="content"/>.</returns>
        protected virtual string GenerateSourceCodeFromView(ISourceContent content, IViewServiceContext context, INaming naming)
        {
            ICollection<IErrorInfo> errors = new List<IErrorInfo>();
            ICodeObject codeObject = ParseViewContent(content, context);

            PreProcessorService.Process(codeObject, new DefaultPreProcessorServiceContext(context.DependencyProvider));
            return GenerateSourceCode(codeObject, context, naming);
        }

        /// <summary>
        /// Gets root property of view.
        /// </summary>
        /// <param name="context">Context information.</param>
        /// <returns>Root property of view.</returns>
        protected virtual IPropertyDescriptor GetRootPropertyDescriptor(IViewServiceContext context)
        {
            IPropertyInfo propertyInfo = new TypePropertyInfo(typeof(GeneratedView).GetProperty(TypeHelper.PropertyName<GeneratedView>(v => v.Content)));
            return new ListAddPropertyDescriptor(propertyInfo);
        }

        /// <summary>
        /// Uses <see cref="IParserService"/> to parse view content and creates AST.
        /// </summary>
        /// <param name="content">Template content.</param>
        /// <param name="context">Context information.</param>
        /// <param name="parserService">Parser service.</param>
        /// <returns>AST.</returns>
        protected virtual ICodeObject ParseViewContent(ISourceContent content, IViewServiceContext context, IParserService parserService = null)
        {
            ICollection<IErrorInfo> errors = new List<IErrorInfo>();
            IPropertyDescriptor contentProperty = GetRootPropertyDescriptor(context);
            ICodeObject codeObject = null;

            DebugHelper.Debug("ParseContent", () =>
            {
                codeObject = (parserService ?? ParserService).ProcessContent(content, new DefaultParserServiceContext(context.DependencyProvider, errors));
                if (codeObject == null)
                    throw new CodeDomViewServiceException("Error parsing view content!", errors, content.TextContent);
            });

            return codeObject;
        }

        /// <summary>
        /// Generates C# source code from AST in <paramref name="codeObject"/>.
        /// </summary>
        /// <param name="codeObject">Root AST object.</param>
        /// <param name="context">Context information.</param>
        /// <param name="naming">Naming for this template compilation.</param>
        /// <param name="codeGeneratorService">Custom generator service to override default in <see cref="CodeGeneratorService"/>.</param>
        /// <returns>Generated C# source code from AST in <paramref name="codeObject"/>.</returns>
        protected virtual string GenerateSourceCode(ICodeObject codeObject, IViewServiceContext context, INaming naming, ICodeGeneratorService codeGeneratorService = null)
        {
            ICollection<IErrorInfo> errors = new List<IErrorInfo>();
            TextWriter writer = new StringWriter();

            DebugHelper.Debug("GenerateSourceCode", () =>
            {
                ICodeGeneratorServiceContext generatorContext = new CodeDomGeneratorServiceContext(naming, writer, context.DependencyProvider, errors);
                bool generatorResult = (codeGeneratorService ?? CodeGeneratorService).GeneratedCode("CSharp", codeObject, generatorContext);
                if (!generatorResult)
                    throw new CodeDomViewServiceException("Error generating code from view!", errors);
            });

            return writer.ToString();
        }

        /// <summary>
        /// Generates javascript source code from AST in <paramref name="contentProperty"/>.
        /// </summary>
        /// <param name="contentProperty">Root AST property.</param>
        /// <param name="context">Context information.</param>
        /// <param name="naming">Naming for this template compilation.</param>
        /// <param name="codeGeneratorService">Custom generator service to override default in <see cref="CodeGeneratorService"/>.</param>
        /// <returns>Generated javascript source code from AST in <paramref name="contentProperty"/>.</returns>
        protected virtual string GenerateJavascriptSourceCode(ICodeObject codeObject, IViewServiceContext context, INaming naming, ICodeGeneratorService codeGeneratorService = null)
        {
            ICollection<IErrorInfo> errors = new List<IErrorInfo>();
            TextWriter writer = new StringWriter();

            DebugHelper.Debug("GenerateSourceCode", () =>
            {
                ICodeGeneratorServiceContext generatorContext = new CodeDomGeneratorServiceContext(naming, writer, context.DependencyProvider, errors);
                bool generatorResult = (codeGeneratorService ?? CodeGeneratorService).GeneratedCode("Javascript", codeObject, generatorContext);
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
            return naming.AssemblyName;
        }
    }
}
