using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeCompilers
{
    /// <summary>
    /// Compiles code and process result object.
    /// </summary>
    public interface ICodeCompiler
    {
        /// <summary>
        /// Compiles <paramref name="sourceCode"/> to resulting object.
        /// </summary>
        /// <param name="sourceCode">Source code to compile.</param>
        /// <param name="context">Context for compilation.</param>
        /// <returns>Resulting object; <c>null</c> if compilation was not successfull.</returns>
        object Compile(TextReader sourceCode, ICodeCompilerContext context);
    }
}
