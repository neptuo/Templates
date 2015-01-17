using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Default implementation of <see cref="ICodeDomPropertyResult"/>
    /// </summary>
    public class DefaultCodeDomPropertyResult : ICodeDomPropertyResult
    {
        public IEnumerable<CodeStatement> Statements { get; private set; }

        public DefaultCodeDomPropertyResult()
        {
            Statements = new List<CodeStatement>();
        }

        public DefaultCodeDomPropertyResult(IEnumerable<CodeStatement> statements)
        {
            Guard.NotNull(statements, "statements");
            Statements = statements;
        }

        public DefaultCodeDomPropertyResult(params CodeStatement[] statements)
        {
            Guard.NotNull(statements, "statements");
            Statements = statements;
        }
    }
}
