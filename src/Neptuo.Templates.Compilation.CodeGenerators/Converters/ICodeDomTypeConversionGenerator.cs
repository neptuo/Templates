using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Generator for converting result of one expression to expression to required type.
    /// </summary>
    public interface ICodeDomTypeConversionGenerator
    {
        /// <summary>
        /// Converts <paramref name="expression"/> (which returns type <paramref name="expressionReturnType"/>) 
        /// to expression (which returns <paramref name="requiredType" />).
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="requiredType">Target type.</param>
        /// <param name="expression">Expression which provides object of type <paramref name="expressionReturnType"/>.</param>
        /// <param name="expressionReturnType">Source type.</param>
        /// <returns>Converted expression which returns object of type <paramref name="requiredType"/>.</returns>
        CodeExpression Generate(ICodeDomContext context, Type requiredType, CodeExpression expression, Type expressionReturnType);
    }
}
