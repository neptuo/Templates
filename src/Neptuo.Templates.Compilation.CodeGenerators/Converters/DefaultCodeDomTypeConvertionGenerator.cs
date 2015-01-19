using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Default implementation of <see cref="ICodeDomTypeConversionGenerator"/> which uses <see cref="Converts"/>.
    /// </summary>
    public class DefaultCodeDomTypeConvertionGenerator : ICodeDomTypeConversionGenerator
    {
        public CodeExpression Generate(ICodeDomContext context, Type requiredType, CodeExpression expression, Type expressionReturnType)
        {
            if (requiredType.IsAssignableFrom(expressionReturnType))
                return expression;

            return GenerateConversion(context, requiredType, expression, expressionReturnType);
        }

        /// <summary>
        /// Invoked when conversion is really needed (types are not assignable).
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="requiredType">Target type.</param>
        /// <param name="expression">Expression which provides object of type <paramref name="expressionReturnType"/>.</param>
        /// <param name="expressionReturnType">Source type.</param>
        /// <returns>Converted expression which returns object of type <paramref name="requiredType"/>.</returns>
        protected virtual CodeExpression GenerateConversion(ICodeDomContext context, Type requiredType, CodeExpression expression, Type expressionReturnType)
        {
            return new CodeMethodInvokeExpression(
                new CodeMethodReferenceExpression(
                    new CodeTypeReferenceExpression(typeof(Converts)),
                    "To",
                    new CodeTypeReference(expressionReturnType),
                    new CodeTypeReference(requiredType)
                ),
                expression
            );
        }
    }
}
