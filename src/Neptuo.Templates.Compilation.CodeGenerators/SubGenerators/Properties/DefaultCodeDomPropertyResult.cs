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
        private List<CodeStatement> statements;

        public IEnumerable<CodeStatement> Statements { get { return statements; } }

        public DefaultCodeDomPropertyResult()
        {
            this.statements = new List<CodeStatement>();
        }

        public DefaultCodeDomPropertyResult(IEnumerable<CodeStatement> statements)
        {
            Guard.NotNull(statements, "statements");
            this.statements = new List<CodeStatement>(statements);
        }

        public DefaultCodeDomPropertyResult(params CodeStatement[] statements)
        {
            Guard.NotNull(statements, "statements");
            this.statements = new List<CodeStatement>(statements);
        }

        /// <summary>
        /// Adds statement to the collection and returns self.
        /// </summary>
        /// <param name="statement">New statement to add to the collection.</param>
        public DefaultCodeDomPropertyResult AddStatement(CodeStatement statement)
        {
            Guard.NotNull(statement, "statement");
            statements.Add(statement);
            return this;
        }
    }
}
