﻿using System;
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

        /// <summary>
        /// Removes <paramref name="lastCharsToRemove"/> from the end of the current content of <paramref name="decorator"/>.
        /// Value of <paramref name="lastCharsToRemove"/> must be positive or zero.
        /// If text is not long enough it returns whole text from current content.
        /// </summary>
        /// <param name="decorator">Content decorator.</param>
        /// <param name="lastCharsToRemove">Chars to remove from the end of the current content of <paramref name="decorator"/>.</param>
        /// <returns>Current content from the <paramref name="decorator"/> without last <paramref name="lastCharsToRemove"/> chars.</returns>
        public static string CurrentContent(this ContentDecorator decorator, int lastCharsToRemove)
        {
            Ensure.NotNull(decorator, "decorator");
            Ensure.PositiveOrZero(lastCharsToRemove, "lastCharsToRemove");

            string text = decorator.CurrentContent();
            if (lastCharsToRemove > 0 && text.Length - lastCharsToRemove > 0)
                text.Substring(0, text.Length - lastCharsToRemove);

            return text;
        }
    }
}
