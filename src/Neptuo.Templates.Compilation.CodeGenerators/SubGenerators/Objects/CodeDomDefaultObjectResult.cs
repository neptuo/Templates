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
        public ICodeDomObjectResult.IExpressionResult Expression { get; private set; }
        public ICodeDomObjectResult.IStatementResult Statement { get; private set; }
        
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

        public class ExpressionResult : ICodeDomObjectResult.IExpressionResult
        {
            public CodeExpression Expression { get; private set; }
            public Type ExpressionReturnType { get; private set; }

            public ExpressionResult(CodeExpression expression, Type expressionReturnType)
            {
                Guard.NotNull(expression, "expression");
                Guard.NotNull(expressionReturnType, "expressionReturnType");
                Expression = expression;
                ExpressionReturnType = expressionReturnType;
            }
        }

        public class StatementResult : ICodeDomObjectResult.IStatementResult
        {
            public CodeStatement Statement { get; private set; }

            public StatementResult(CodeStatement statement)
            {
                Guard.NotNull(statement, "statement");
                Statement = statement;
            }
        }
    }
}
