﻿using Neptuo.Diagnostics;
using Neptuo.Templates.Compilation.CodeCompilers;
using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Templates.Compilation.PreProcessing;
using Neptuo.Templates.Compilation.ViewActivators;
using Neptuo.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Base implementation of <see cref="IViewService"/>.
    /// Support multi processes.
    /// </summary>
    public class DefaultViewService : DebugBase, IViewService
    {
        private readonly MultiLockProvider lockProvider;

        /// <summary>
        /// Service for parsing view content.
        /// </summary>
        public IParserService ParserService { get; private set; }

        /// <summary>
        /// Service for pre-processing AST.
        /// </summary>
        public IPreProcessorService PreProcessorService { get; private set; }

        /// <summary>
        /// Service for generating source code.
        /// </summary>
        public ICodeGeneratorService GeneratorService { get; private set; }

        /// <summary>
        /// Service for compiling source code.
        /// </summary>
        public ICodeCompilerService CompilerService { get; private set; }

        /// <summary>
        /// Service for activating (creating instances) of compiled views.
        /// </summary>
        public IViewActivatorService ActivatorService { get; private set; }

        /// <summary>
        /// Pipeline name dispatcher.
        /// Provides ability to rename pipeline for each part of processing template.
        /// </summary>
        public DefaultPipelineDispatcher Pipeline { get; private set; }

        /// <summary>
        /// Text writer for simple performance measurements.
        /// </summary>
        public DebugBase.DebugMessageWriter DebugWriter
        {
            get { return InnerWriter; }
            set
            {
                if (value != null)
                    InnerWriter = value;
                else
                    InnerWriter = (format, parameters) => { };
            }
        }

        /// <summary>
        /// Creates new empty instance.
        /// </summary>
        /// <param name="keyMapper">
        /// Function which is used to map keys for <see cref="MultiLockProvider"/>. 
        /// This provider is used for exclusive template content compilation.
        /// </param>
        public DefaultViewService(Func<object, string> keyMapper = null)
            : base(Console.Out)
        {
            ParserService = new DefaultParserService();
            PreProcessorService = new DefaultPreProcessorService();
            GeneratorService = new DefaultCodeGeneratorService();
            CompilerService = new DefaultCodeCompilerService();
            ActivatorService = new DefaultViewActivatorService();
            Pipeline = new DefaultPipelineDispatcher();

            if (keyMapper == null)
                lockProvider = new MultiLockProvider();
            else
                lockProvider = new MultiLockProvider(keyMapper);
        }

        public object ProcessContent(string name, ISourceContent content, IViewServiceContext context)
        {
            Ensure.NotNullOrEmpty(name, "name");
            Ensure.NotNull(content, "content");
            Ensure.NotNull(context, "context");

            // Try already compiled view.
            object compiledView = ExecuteActivatorService(name, content, context);

            // If instance can't be created (eg. not compiled), try to compile view.
            if (compiledView == null)
            {
                // Lock view compilation
                using (lockProvider.Lock(content))
                {
                    // Try already compiled view.
                    compiledView = ExecuteActivatorService(name, content, context);

                    // If instance can't be created (eg. not compiled), try to compile view.
                    if (compiledView == null)
                    {
                        // Parse view content.
                        ICodeObject codeObject = ExecuteParserService(name, content, context);
                        if (codeObject == null)
                            return null;

                        // Pre-process AST.
                        ExecutePreProcessorService(name, codeObject, context);

                        // Generate source code.
                        StringWriter sourceCode = new StringWriter();
                        bool codeGenerationResult = ExecuteGeneratorService(name, sourceCode, codeObject, context);
                        if (!codeGenerationResult)
                            return null;

                        // Compile source code.
                        compiledView = ExecuteCompilerService(name, new StringReader(sourceCode.ToString()), context);
                    }
                }
            }

            // Return compiled view, if null, compilation was not successfull.
            return compiledView;
        }

        private object ExecuteActivatorService(string name, ISourceContent content, IViewServiceContext context)
        {
            name = Pipeline.DispatchViewActivatorService(name);
            return ActivatorService.Activate(name, content, new DefaultViewActivatorServiceContext(context.DependencyProvider, context.Errors));
        }

        private ICodeObject ExecuteParserService(string name, ISourceContent content, IViewServiceContext context)
        {
            name = Pipeline.DispatchParserService(name);
            return ParserService.ProcessContent(name, content, new DefaultParserServiceContext(context.DependencyProvider, context.Errors));
        }

        private void ExecutePreProcessorService(string name, ICodeObject codeObject, IViewServiceContext context)
        {
            name = Pipeline.DispatchPreProcessorService(name);
            PreProcessorService.Process(codeObject, new DefaultPreProcessorServiceContext(context.DependencyProvider));
        }

        private bool ExecuteGeneratorService(string name, TextWriter sourceCode, ICodeObject codeObject, IViewServiceContext context)
        {
            name = Pipeline.DispatchCodeGeneratorService(name);
            return GeneratorService.GeneratedCode(name, codeObject, new DefaultCodeGeneratorServiceContext(sourceCode, context.DependencyProvider, context.Errors));
        }

        private object ExecuteCompilerService(string name, TextReader sourceCode, IViewServiceContext context)
        {
            name = Pipeline.DispatchCodeCompilerService(name);
            return CompilerService.Compile(name, sourceCode, new DefaultCodeCompilerServiceContext(context.DependencyProvider, context.Errors));
        }
    }
}
