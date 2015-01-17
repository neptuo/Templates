using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public static class _CodeDomObjectPropertyExtensions
    {
        /// <summary>
        /// Returns <c>true</c> if <paramref name="result"/> has statement; otherwise returns <c>false</c>.
        /// </summary>
        /// <returns><c>true</c> if <paramref name="result"/> has statement; otherwise returns <c>false</c>.</returns>
        public static bool HasStatement(this ICodeDomPropertyResult result)
        {
            Guard.NotNull(result, "result");
            return result.Statements.Any();
        }
    }
}
