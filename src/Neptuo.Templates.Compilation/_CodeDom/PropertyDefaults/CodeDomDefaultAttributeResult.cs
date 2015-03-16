using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Default implementation of <see cref="ICodeDomAttributeResult"/>.
    /// </summary>
    public class CodeDomDefaultAttributeResult : ICodeDomAttributeResult
    {
        public CodeExpression Expression { get; private set; }

        public CodeDomDefaultAttributeResult()
        { }

        public CodeDomDefaultAttributeResult(CodeExpression expression)
        {
            Ensure.NotNull(expression, "expression");
            Expression = expression;
        }
    }
}
