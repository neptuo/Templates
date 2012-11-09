using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Utils;

namespace TestConsole
{
    class TestConverter
    {
        public static void Test()
        {
            Console.WriteLine(StringConverter.Convert("125", typeof(int)));
            Console.WriteLine(StringConverter.Convert(null, typeof(int)));
            Console.WriteLine(StringConverter.Convert(null, typeof(int?)));
            Console.WriteLine(StringConverter.Convert("125", typeof(String)));
            Console.WriteLine(StringConverter.Convert(null, typeof(String)));
            Console.WriteLine(StringConverter.Convert(null, typeof(bool)));
            Console.WriteLine(StringConverter.Convert("125", typeof(bool)));
            Console.WriteLine(StringConverter.Convert("true", typeof(bool)));
            Console.WriteLine(StringConverter.Convert(null, typeof(TestEnum)));
            Console.WriteLine(StringConverter.Convert("XX", typeof(TestEnum)));
            Console.WriteLine(StringConverter.Convert("One", typeof(TestEnum)));
        }

        enum TestEnum
        {
            One, Two, Three
        }
    }
}
