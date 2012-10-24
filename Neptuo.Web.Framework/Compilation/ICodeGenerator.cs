using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    /// <summary>
    /// Code generator that processes AST and generates code for view.
    /// </summary>
    public interface ICodeGenerator
    {
        /// <summary>
        /// Processes <paramref name="rootObject"/> and generates code for view.
        /// </summary>
        /// <param name="rootObject">Root AST object.</param>
        /// <param name="context">Context</param>
        bool ProcessTree(IPropertyDescriptor propertyDescriptor, ICodeGeneratorContext context);
    }
}
