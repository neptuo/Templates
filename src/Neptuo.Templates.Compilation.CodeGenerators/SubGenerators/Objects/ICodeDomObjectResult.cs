using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Describes result from <see cref="ICodeDomObjectGenerator"/>.
    /// </summary>
    public interface ICodeDomObjectResult
    {
        /// <summary>
        /// Expression from code object.
        /// </summary>
        CodeExpression Expression { get; }

        /// <summary>
        /// Type return from <see cref="ICodeObjectResult.Expression"/>.
        /// </summary>
        Type ExpressionReturnType { get; }
    }
}
