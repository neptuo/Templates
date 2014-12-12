using Neptuo.Compilers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeCompilers
{
    /// <summary>
    /// Base for default implementation for compiling C# source code to the assemblies.
    /// </summary>
    public abstract class StaticCodeCompilerBase : ICodeCompiler, ICompilerConfiguration
    {
        private readonly CompilerFactory compilerFactory;
        private readonly string tempDirectory;

        /// <summary>
        /// Whether debug mode is on/off (.cs saving, pdb generation, etc.)
        /// </summary>
        public bool IsDebugMode
        {
            get { return compilerFactory.IsDebugMode; }
            set { compilerFactory.IsDebugMode = value; }
        }

        /// <summary>
        /// Compiler references.
        /// </summary>
        public CompilerReferenceCollection References
        {
            get { return compilerFactory.References; }
        }

        /// <summary>
        /// Creates new instance which stores temp files to the <paramref name="tempDirectory"/>.
        /// </summary>
        /// <param name="tempDirectory">Directory for storing temp files (.cs, .dll, .pdb, etc).</param>
        public StaticCodeCompilerBase(string tempDirectory)
        {
            Guard.NotNullOrEmpty(tempDirectory, "tempDirectory");

            if (!Directory.Exists(tempDirectory))
                throw Guard.Exception.ArgumentDirectoryNotExist(tempDirectory, "tempDirectory");

            this.compilerFactory = new CompilerFactory();
            this.tempDirectory = tempDirectory;
        }

        public object Compile(TextReader sourceCode, ICodeCompilerContext context)
        {
            return Compile(compilerFactory.CreateStatic(), tempDirectory, sourceCode, context);
        }

        /// <summary>
        /// Should try to compile <paramref name="sourceCode"/> into <paramref name="tempDirectory"/> using <paramref name="compiler."/>
        /// </summary>
        /// <param name="compiler">C# code compiler.</param>
        /// <param name="tempDirectory">Directory for storing temp files (.cs, .dll, .pdb, etc).</param>
        /// <param name="sourceCode">C# source code.</param>
        /// <param name="context">Code compiler context.</param>
        /// <returns>Compiled view instance.</returns>
        protected abstract object Compile(IStaticCompiler compiler, string tempDirectory, TextReader sourceCode, ICodeCompilerContext context);
    }
}