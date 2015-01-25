using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Describes result of <see cref="ICodeDomPropertyTypeGenerator"/>.
    /// </summary>
    public interface ICodeDomPropertyTypeResult
    {
        /// <summary>
        /// Generated expression.
        /// </summary>
        CodeExpression Expression { get; }
    }
}
