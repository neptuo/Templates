using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Describes statement result from <see cref="ICodeDomObjectGenerator"/>.
    /// </summary>
    public interface ICodeDomObjectStatementResult
    {
        /// <summary>
        /// Statement from code object.
        /// </summary>
        CodeStatement Value { get; }
    }
}
