using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    public static class _CodePropertyExtensions
    {
        /// <summary>
        /// Sets all values from <paramref name="values"/> to the <paramref name="codeProperty"/>.
        /// </summary>
        /// <param name="codeProperty">Target property.</param>
        /// <param name="values">Enumeration of values to set to the <paramref name="codeProperty"/>.</param>
        public static void SetRangeValue(this ICodeProperty codeProperty, IEnumerable<ICodeObject> values)
        {
            Guard.NotNull(codeProperty, "codeProperty");
            Guard.NotNull(values, "values");

            foreach (ICodeObject value in values)
                codeProperty.SetValue(value);
        }

        /// <summary>
        /// Sets all values from <paramref name="values"/> to the <paramref name="codeProperty"/>.
        /// </summary>
        /// <param name="codeProperty">Target property.</param>
        /// <param name="values">Enumeration of values to set to the <paramref name="codeProperty"/>.</param>
        public static void SetRangeValue(this ICodeProperty codeProperty, params ICodeObject[] values)
        {
            Guard.NotNull(codeProperty, "codeProperty");
            Guard.NotNull(values, "values");

            foreach (ICodeObject value in values)
                codeProperty.SetValue(value);
        }
    }
}
