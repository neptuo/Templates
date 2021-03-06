﻿using System;
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
    public class CodeDomDefaultPropertyResult : ICodeDomPropertyResult
    {
        private List<CodeStatement> statements;

        public IEnumerable<CodeStatement> Statements { get { return statements; } }

        public CodeDomDefaultPropertyResult()
        {
            this.statements = new List<CodeStatement>();
        }

        public CodeDomDefaultPropertyResult(IEnumerable<CodeStatement> statements)
        {
            Ensure.NotNull(statements, "statements");
            this.statements = new List<CodeStatement>(statements);
        }

        public CodeDomDefaultPropertyResult(params CodeStatement[] statements)
        {
            Ensure.NotNull(statements, "statements");
            this.statements = new List<CodeStatement>(statements);
        }

        /// <summary>
        /// Adds statement to the collection and returns self.
        /// </summary>
        /// <param name="statement">New statement to add to the collection.</param>
        public CodeDomDefaultPropertyResult AddStatement(CodeStatement statement)
        {
            Ensure.NotNull(statement, "statement");
            statements.Add(statement);
            return this;
        }
    }
}
