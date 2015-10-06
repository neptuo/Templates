using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax
{
    /// <summary>
    /// Common extensions for <see cref="ICodePropertyBuilderContext"/>.
    /// </summary>
    public static class _CodePropertyBuilderContextExtensions
    {
        /// <summary>
        /// Creates instance of <see cref="ICodePropertyBuilderContext"/>.
        /// </summary>
        /// <param name="context">Code property builde context.</param>
        /// <returns>Instance of <see cref="ICodePropertyBuilderContext"/>.</returns>
        public static ICodeObjectBuilderContext CreateObjectContext(this ICodePropertyBuilderContext context)
        {
            Ensure.NotNull(context, "context");
            return new DefaultCodeObjectBuilderContext(context.ParserProvider);
        }
    }
}
