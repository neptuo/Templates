using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Usefull extensions for <see cref="System.CodeDom"/>.
    /// </summary>
    public static class _CodeDomExtensions
    {
        public static CodeStatementCollection AddRange(this CodeStatementCollection collection, IEnumerable<CodeStatement> statements)
        {
            Guard.NotNull(collection, "collection");
            Guard.NotNull(statements, "statements");

            foreach (CodeStatement statement in statements)
                collection.Add(statement);

            return collection;
        }
    }
}
