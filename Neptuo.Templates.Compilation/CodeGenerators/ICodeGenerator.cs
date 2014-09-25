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
        /// Processes <paramref name="propertyDescriptor"/> and generates code for view.
        /// </summary>
        /// <param name="propertyDescriptor">Root AST property.</param>
        /// <param name="context">Context</param>
        bool ProcessTree(IPropertyDescriptor propertyDescriptor, ICodeGeneratorContext context);
    }
}
