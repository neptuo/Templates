using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers.IO
{
    /// <summary>
    /// Common extensions for <see cref="ContentDecorator"/>.
    /// </summary>
    public static class _ContentDecoratorExtensions
    {
        /// <summary>
        /// Reads content from <paramref name="decorator"/> WHILE <paramref name="terminator"/> returns <c>true</c>.
        /// </summary>
        /// <param name="decorator">Content decorator.</param>
        /// <param name="terminator">Function to determine when stop reading.</param>
        /// <returns><c>true</c> if terminated by <paramref name="terminator"/>; <c>false</c> if terminated by EOF (from <paramref name="decorator"/>).</returns>
        public static bool ReadWhile(this ContentDecorator decorator, Func<char, bool> terminator)
        {
            Ensure.NotNull(decorator, "decorator");
            Ensure.NotNull(terminator, "terminator");

            while (decorator.Next())
            {
                if (!terminator(decorator.Current))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Reads content from <paramref name="decorator"/> UNTIL <paramref name="terminator"/> returns <c>true</c>.
        /// </summary>
        /// <param name="decorator">Content decorator.</param>
        /// <param name="terminator">Function to determine when stop reading.</param>
        /// <returns><c>true</c> if terminated by <paramref name="terminator"/>; <c>false</c> if terminated by EOF (from <paramref name="decorator"/>).</returns>
        public static bool ReadUntil(this ContentDecorator decorator, Func<char, bool> terminator)
        {
            Ensure.NotNull(decorator, "decorator");
            Ensure.NotNull(terminator, "terminator");

            while (decorator.Next())
            {
                if (terminator(decorator.Current))
                    return true;
            }

            return false;
        }
    }
}
