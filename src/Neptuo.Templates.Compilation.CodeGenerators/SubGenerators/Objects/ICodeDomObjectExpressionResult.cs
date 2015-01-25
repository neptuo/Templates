using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Describes expression result from <see cref="ICodeDomObjectGenerator"/>.
    /// </summary>
    public interface ICodeDomObjectExpressionResult
    {
        /// <summary>
        /// Expression from code object.
        /// </summary>
        CodeExpression Value { get; }

        /// <summary>
        /// Type return from <see cref="ICodeDomObjectExpressionResult.Value"/>.
        /// </summary>
        Type ReturnType { get; }
    }
}
