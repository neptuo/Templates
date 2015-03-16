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
        public CodeExpression Expression { get; private set; }
        public CodeStatement Statement { get; private set; }
        
        public CodeDomDefaultObjectResult()
        { }

        public CodeDomDefaultObjectResult(CodeExpression expression, Type expressionReturnType)
        {
            Ensure.NotNull(expression, "expression");
            Ensure.NotNull(expressionReturnType, "expressionReturnType");
            Expression = expression;
            Expression.AddReturnType(expressionReturnType);
        }

        public CodeDomDefaultObjectResult(CodeStatement statement)
        {
            Ensure.NotNull(statement, "statement");
            Statement = statement;
        }
    }
}
