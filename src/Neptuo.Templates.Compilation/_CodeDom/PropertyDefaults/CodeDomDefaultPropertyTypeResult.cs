using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Default implementation of <see cref="ICodeDomPropertyTypeResult"/>.
    /// </summary>
    public class CodeDomDefaultPropertyTypeResult : ICodeDomPropertyTypeResult
    {
        public CodeExpression Expression { get; private set; }

        public CodeDomDefaultPropertyTypeResult()
        { }

        public CodeDomDefaultPropertyTypeResult(CodeExpression expression)
        {
            Ensure.NotNull(expression, "expression");
            Expression = expression;
        }
    }
}
