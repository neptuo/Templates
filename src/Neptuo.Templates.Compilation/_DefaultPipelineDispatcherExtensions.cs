using Neptuo.Templates.Compilation.CodeCompilers;
using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Templates.Compilation.PreProcessing;
using Neptuo.Templates.Compilation.ViewActivators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    public static class _DefaultPipelineDispatcherExtensions
    {
        #region IParserService

        public static DefaultPipelineDispatcher AddParserService(this DefaultPipelineDispatcher pipeline, string sourceName, string targetName)
        {
            Ensure.NotNull(pipeline, "pipeline");
            return pipeline.Add<IParserService>(sourceName, targetName);
        }

        public static string DispatchParserService(this DefaultPipelineDispatcher pipeline, string sourceName)
        {
            Ensure.NotNull(pipeline, "pipeline");
            return pipeline.Dispatch<IParserService>(sourceName);
        }

        #endregion

        #region IViewActivatorService

        public static DefaultPipelineDispatcher AddViewActivatorService(this DefaultPipelineDispatcher pipeline, string sourceName, string targetName)
        {
            Ensure.NotNull(pipeline, "pipeline");
            return pipeline.Add<IViewActivatorService>(sourceName, targetName);
        }

        public static string DispatchViewActivatorService(this DefaultPipelineDispatcher pipeline, string sourceName)
        {
            Ensure.NotNull(pipeline, "pipeline");
            return pipeline.Dispatch<IViewActivatorService>(sourceName);
        }

        #endregion

        #region IPreProcessorService

        public static DefaultPipelineDispatcher AddPreProcessorService(this DefaultPipelineDispatcher pipeline, string sourceName, string targetName)
        {
            Ensure.NotNull(pipeline, "pipeline");
            return pipeline.Add<IPreProcessorService>(sourceName, targetName);
        }

        public static string DispatchPreProcessorService(this DefaultPipelineDispatcher pipeline, string sourceName)
        {
            Ensure.NotNull(pipeline, "pipeline");
            return pipeline.Dispatch<IPreProcessorService>(sourceName);
        }

        #endregion

        #region ICodeGeneratorService

        public static DefaultPipelineDispatcher AddCodeGeneratorService(this DefaultPipelineDispatcher pipeline, string sourceName, string targetName)
        {
            Ensure.NotNull(pipeline, "pipeline");
            return pipeline.Add<ICodeGeneratorService>(sourceName, targetName);
        }

        public static string DispatchCodeGeneratorService(this DefaultPipelineDispatcher pipeline, string sourceName)
        {
            Ensure.NotNull(pipeline, "pipeline");
            return pipeline.Dispatch<ICodeGeneratorService>(sourceName);
        }

        #endregion

        #region ICodeCompilerService

        public static DefaultPipelineDispatcher AddCodeCompilerService(this DefaultPipelineDispatcher pipeline, string sourceName, string targetName)
        {
            Ensure.NotNull(pipeline, "pipeline");
            return pipeline.Add<ICodeCompilerService>(sourceName, targetName);
        }

        public static string DispatchCodeCompilerService(this DefaultPipelineDispatcher pipeline, string sourceName)
        {
            Ensure.NotNull(pipeline, "pipeline");
            return pipeline.Dispatch<ICodeCompilerService>(sourceName);
        }

        #endregion
    }
}
