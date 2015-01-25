using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Describes result of <see cref="ICodeDomAttributeGenerator"/>.
    /// </summary>
    public interface ICodeDomAttributeResult
    {
        /// <summary>
        /// Generated expression.
        /// </summary>
        CodeExpression Expression { get; }
    }
}
