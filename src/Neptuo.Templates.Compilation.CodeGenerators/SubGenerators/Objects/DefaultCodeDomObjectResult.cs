using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Default implmentation of <see cref="ICodeDomObjectResult"/>.
    /// </summary>
    public class DefaultCodeDomObjectResult : ICodeDomObjectResult
    {
        public CodeExpression Expression { get; private set; }
        public Type ExpressionReturnType { get; private set; }

        public DefaultCodeDomObjectResult(CodeExpression expression, Type expressionReturnType)
        {
            Guard.NotNull(expression, "expression");
            Guard.NotNull(expressionReturnType, "expressionReturnType");
            Expression = expression;
            ExpressionReturnType = expressionReturnType;
        }
    }
}
