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
    public class CodeDomDefaultObjectResult : ICodeDomObjectResult
    {
        public ICodeDomObjectExpressionResult Expression { get; private set; }
        public ICodeDomObjectStatementResult Statement { get; private set; }
        
        public CodeDomDefaultObjectResult()
        { }

        public CodeDomDefaultObjectResult(CodeExpression expression, Type expressionReturnType)
        {
            Expression = new ExpressionResult(expression, expressionReturnType);
        }

        public CodeDomDefaultObjectResult(CodeStatement statement)
        {
            Statement = new StatementResult(statement);
        }

        public class ExpressionResult : ICodeDomObjectExpressionResult
        {
            public CodeExpression Value { get; private set; }
            public Type ReturnType { get; private set; }

            public ExpressionResult(CodeExpression expression, Type expressionReturnType)
            {
                Guard.NotNull(expression, "expression");
                Guard.NotNull(expressionReturnType, "expressionReturnType");
                Value = expression;
                ReturnType = expressionReturnType;
            }
        }

        public class StatementResult : ICodeDomObjectStatementResult
        {
            public CodeStatement Value { get; private set; }

            public StatementResult(CodeStatement statement)
            {
                Guard.NotNull(statement, "statement");
                Value = statement;
            }
        }
    }
}
