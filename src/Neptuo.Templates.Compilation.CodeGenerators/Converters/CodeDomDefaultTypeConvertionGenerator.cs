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
    public class CodeDomDefaultTypeConvertionGenerator : ICodeDomTypeConversionGenerator
    {
        /// <summary>
        /// If <paramref name="expression"/> is primitive, calls <see cref="CodeDomDefaultTypeConvertionGenerator.GenerateCompileTimeConversion"/>;
        /// otherwise calls <see cref="CodeDomDefaultTypeConvertionGenerator.GenerateRuntimeConversion"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requiredType"></param>
        /// <param name="expression"></param>
        /// <param name="expressionReturnType"></param>
        /// <returns></returns>
        public CodeExpression Generate(ICodeDomContext context, Type requiredType, CodeExpression expression, Type expressionReturnType)
        {
            // If current expression provides assignable value, simply return it.
            if (requiredType.IsAssignableFrom(expressionReturnType))
                return expression;

            // If expression is primitive, do compile time conversion.
            CodePrimitiveExpression primitiveExpression = expression as CodePrimitiveExpression;
            if (primitiveExpression != null)
            {
                // If null, return it.
                if (primitiveExpression.Value == null)
                    return primitiveExpression;

                // Do compile time conversion.
                return GenerateCompileTimeConversion(context, requiredType, primitiveExpression.Value, expressionReturnType);
            }

            // Otherwise do runtime conversion.
            return GenerateRuntimeConversion(context, requiredType, expression, expressionReturnType);
        }

        /// <summary>
        /// Generates conversion from constant at compile time.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="sourceValue">Source contant value.</param>
        /// <returns>Converted expression which returns object of type <paramref name="targetType"/>.</returns>
        protected virtual CodeExpression GenerateCompileTimeConversion(ICodeDomContext context, Type targetType, object sourceValue, Type sourceType)
        {
            object targetValue;
            if (!Converts.Try(sourceType, targetType, sourceValue, out targetValue))
            {
                context.AddError("Target type ('{0}') can't be constructed from value '{1}'.", targetType.FullName, sourceValue);
                return null;
            }

            return new CodePrimitiveExpression(targetValue);
        }

        /// <summary>
        /// Generates conversion from expression at runtime.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="requiredType">Target type.</param>
        /// <param name="expression">Expression which provides object of type <paramref name="expressionReturnType"/>.</param>
        /// <param name="expressionReturnType">Source type.</param>
        /// <returns>Converted expression which returns object of type <paramref name="requiredType"/>.</returns>
        protected virtual CodeExpression GenerateRuntimeConversion(ICodeDomContext context, Type requiredType, CodeExpression expression, Type expressionReturnType)
        {
            return new CodeCastExpression(
                new CodeTypeReference(requiredType),
                new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression(typeof(Converts)),
                    "To",
                    new CodeTypeOfExpression(requiredType),
                    expression
                )
            );
        }
    }
}
