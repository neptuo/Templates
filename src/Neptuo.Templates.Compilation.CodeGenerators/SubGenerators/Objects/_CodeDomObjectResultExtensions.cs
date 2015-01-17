using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public static class _CodeDomObjectResultExtensions
    {
        /// <summary>
        /// Returns <c>true</c> if <paramref name="result"/> has expression not <c>null</c>; otherwise returns <c>false</c>.
        /// </summary>
        /// <returns><c>true</c> if <paramref name="result"/> has expression not <c>null</c>; otherwise returns <c>false</c>.</returns>
        public static bool HasExpression(this ICodeDomObjectResult result)
        {
            Guard.NotNull(result, "result");
            return result.Expression != null;
        }
    }
}
