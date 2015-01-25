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
    /// One of <see cref="ICodeDomObjectResult.Expression"/> or <see cref="ICodeDomObjectResult.Statement"/> 
    /// if something was generated.
    /// If both are <c>null</c> than processing was successfull, but nothing was generated.
    /// </summary>
    public interface ICodeDomObjectResult
    {
        /// <summary>
        /// Result expression describtion.
        /// </summary>
        CodeExpression Expression { get; }

        /// <summary>
        /// Result statement describtion.
        /// </summary>
        CodeStatement Statement { get; }
    }
}
