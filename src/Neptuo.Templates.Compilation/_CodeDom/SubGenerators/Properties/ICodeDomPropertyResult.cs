using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Describes result from <see cref="ICodeDomPropertyGenerator"/>.
    /// </summary>
    public interface ICodeDomPropertyResult
    {
        /// <summary>
        /// Enumeration of created statements.
        /// </summary>
        IEnumerable<CodeStatement> Statements { get; }
    }
}
