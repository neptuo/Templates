using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.PreProcessing
{
    /// <summary>
    /// AST pre processing service.
    /// Pre processes AST before generating code.
    /// </summary>
    public interface IPreProcessorService
    {
        /// <summary>
        /// Registers visitor.
        /// </summary>
        /// <param name="visitor">New visitor.</param>
        void AddVisitor(IVisitor visitor);

        /// <summary>
        /// Use registered visitors to wall through <paramref name="codeObject"/>.
        /// </summary>
        /// <param name="codeObject">Root AST object.</param>
        /// <param name="context">Context information.</param>
        void Process(ICodeObject codeObject, IPreProcessorServiceContext context);
    }
}
