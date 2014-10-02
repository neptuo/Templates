using Neptuo.Diagnostics;
using Neptuo.Templates.Compilation.CodeCompilers;
using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Templates.Compilation.PreProcessing;
using Neptuo.Templates.Compilation.ViewActivators;
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
        /// Text writer pro simple performance measurements.
        /// Defaults to <see cref="Console.Out"/>.
        /// </summary>
        public TextWriter DebugWriter
        {
            get { return InnerWriter; }
            set
            {
                if (value != null)
                    InnerWriter = value;
            }
        }

        /// <summary>
        /// Creates new empty instance.
        /// </summary>
        public DefaultViewService()
            : base(Console.Out)
        {
            ParserService = new DefaultParserService();
            PreProcessorService = new DefaultPreProcessorService();
            GeneratorService = new DefaultCodeGeneratorService();
            CompilerService = new DefaultCodeCompilerService();
            ActivatorService = new DefaultViewActivatorService();
        }

        public object ProcessContent(string name, string viewContent, IViewServiceContext context)
        {
            Guard.NotNullOrEmpty(name, "name");
            Guard.NotNullOrEmpty(viewContent, "viewContent");
            Guard.NotNull(context, "context");

            // Try already compiled view.
            object compiledView = ActivatorService.Activate(name, viewContent, new DefaultViewActivatorServiceContext(context.DependencyProvider, context.Errors));

            // If instance can't be created (eg. not compiled), try to compile view.
            if (compiledView == null)
            {
                // Parse view content.
                ICodeObject codeObject = ParserService.ProcessContent(viewContent, new DefaultParserServiceContext(context.DependencyProvider, context.Errors));
                if (codeObject == null)
                    return null;

                // Pre-process AST.
                PreProcessorService.Process(codeObject, new DefaultPreProcessorServiceContext(context.DependencyProvider));

                // Generate source code.
                StringWriter sourceCode = new StringWriter();
                bool codeGenerationResult = GeneratorService.GeneratedCode(name, codeObject, new DefaultCodeGeneratorServiceContext(sourceCode, context.DependencyProvider, context.Errors));
                if (!codeGenerationResult)
                    return null;

                // Compile source code.
                compiledView = CompilerService.Compile(name, new StringReader(sourceCode.ToString()), new DefaultCodeCompilerServiceContext(context.DependencyProvider, context.Errors));
            }

            // Return compiled view, if null, compilation was not successfull.
            return compiledView;
        }
    }
}
