using Neptuo.Collections.Specialized;
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

        /// <summary>
        /// Creates new instance.
        /// </summary>
        public StaticCodeCompilerBase()
        {
            this.compilerFactory = new CompilerFactory();
        }

        public object Compile(TextReader sourceCode, ICodeCompilerContext context)
        {
            return Compile(compilerFactory.CreateStatic(), sourceCode, context);
        }

        /// <summary>
        /// Should try to compile <paramref name="sourceCode"/> using <paramref name="compiler"/>.
        /// </summary>
        /// <param name="compiler">C# code compiler.</param>
        /// <param name="sourceCode">C# source code.</param>
        /// <param name="context">Code compiler context.</param>
        /// <returns>Compiled view instance.</returns>
        protected abstract object Compile(IStaticCompiler compiler, TextReader sourceCode, ICodeCompilerContext context);

        public IKeyValueCollection Set(string key, object value)
        {
            compilerFactory.Set(key, value);
            return this;
        }

        #region ICompilerConfiguration

        public IEnumerable<string> Keys
        {
            get { return compilerFactory.Keys; }
        }

        public bool TryGet<T>(string key, out T value)
        {
            return compilerFactory.TryGet(key, out value);
        }

        public ICompilerConfiguration Clone()
        {
            return compilerFactory.Clone();
        }

        #endregion
    }
}