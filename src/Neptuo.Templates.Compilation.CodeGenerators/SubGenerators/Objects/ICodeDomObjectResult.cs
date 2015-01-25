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
        IExpressionResult Expression { get; }

        /// <summary>
        /// Result statement describtion.
        /// </summary>
        IStatementResult Statement { get; }

        /// <summary>
        /// Describes expression result from <see cref="ICodeDomObjectGenerator"/>.
        /// </summary>
        public interface IExpressionResult
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

        public interface IStatementResult
        {
            /// <summary>
            /// Statement from code object.
            /// </summary>
            CodeStatement Statement { get; }
        }
    }
}
