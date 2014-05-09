using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Neptuo.Templates
{
    /// <summary>
    /// Stopwatch helper.
    /// </summary>
    public static class DebugUtils
    {
        /// <summary>
        /// Measures length of <paramref name="action"/> writes it into console.
        /// </summary>
        /// <param name="caption">Action caption.</param>
        /// <param name="action">Action to measure.</param>
        public static void Run(string caption, Action action)
        {
            Guard.NotNullOrEmpty(caption, "caption");
            Guard.NotNull(action, "action");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            action();

            stopwatch.Stop();
            Console.WriteLine("'{0}' in {1}ms", caption, stopwatch.ElapsedMilliseconds);
        }
    }
}
