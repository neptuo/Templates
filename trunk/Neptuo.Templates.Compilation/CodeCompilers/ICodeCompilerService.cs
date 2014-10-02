using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeCompilers
{
    /// <summary>
    /// Service for final processing part - source code compilation.
    /// </summary>
    public interface ICodeCompilerService
    {
        /// <summary>
        /// Registers <paramref name="compiler"/> with <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name of compiler.</param>
        /// <param name="compiler">Code compiler.</param>
        ICodeCompilerService AddCompiler(string name, ICodeCompiler compiler);

        /// <summary>
        /// Compiles <paramref name="sourceCode"/> into resulting object.
        /// </summary>
        /// <param name="name">Name of compiler to use.</param>
        /// <param name="sourceCode">Source code to compile.</param>
        /// <param name="context">Context for compilation.</param>
        /// <returns>Resulting object; <c>null</c> if compilation was no successfull.</returns>
        object Compile(string name, TextReader sourceCode, ICodeCompilerServiceContext context);
    }
}
