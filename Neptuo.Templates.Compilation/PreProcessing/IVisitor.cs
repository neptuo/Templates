using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.PreProcessing
{
    /// <summary>
    /// AST visitor.
    /// </summary>
    public interface IVisitor
    {
        /// <summary>
        /// Visits property in <paramref name="propertyDescriptor"/>.
        /// </summary>
        /// <param name="propertyDescriptor">AST property.</param>
        /// <param name="context">Context information.</param>
        void Visit(ICodeObject codeObject, IVisitorContext context);
    }
}
