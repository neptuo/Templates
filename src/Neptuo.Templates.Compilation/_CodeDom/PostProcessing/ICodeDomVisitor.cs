using Neptuo.Templates.Compilation.CodeGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Post code generation code dom structure visitor.
    /// </summary>
    public interface ICodeDomVisitor
    {
        /// <summary>
        /// Can process structure in <see cref="ICodeDomStructure"/> to modify generated code.
        /// </summary>
        /// <param name="context">Visitor context.</param>
        void Visit(ICodeDomContext context);
    }
}
