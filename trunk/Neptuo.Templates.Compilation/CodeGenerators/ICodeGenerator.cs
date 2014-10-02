using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Code generator that processes AST and generates code for view.
    /// </summary>
    public interface ICodeGenerator
    {
        /// <summary>
        /// Processes <paramref name="codeObject"/> and generates code for view.
        /// </summary>
        /// <param name="codeObject">Root AST object.</param>
        /// <param name="context">Context</param>
        bool ProcessTree(ICodeObject codeObject, ICodeGeneratorContext context);
    }
}
