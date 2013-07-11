using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    public static class DebugUtils
    {
        public static void Run(string caption, Action action)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            action();

            stopwatch.Stop();
            Console.WriteLine("'{0}' in {1}ms", caption, stopwatch.ElapsedMilliseconds);
        }
    }
}
