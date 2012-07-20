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
            Console.WriteLine(TypeConverter.Convert("125", typeof(int)));
            Console.WriteLine(TypeConverter.Convert(null, typeof(int)));
            Console.WriteLine(TypeConverter.Convert(null, typeof(int?)));
            Console.WriteLine(TypeConverter.Convert("125", typeof(String)));
            Console.WriteLine(TypeConverter.Convert(null, typeof(String)));
            Console.WriteLine(TypeConverter.Convert(null, typeof(bool)));
            Console.WriteLine(TypeConverter.Convert("125", typeof(bool)));
            Console.WriteLine(TypeConverter.Convert("true", typeof(bool)));
            //Console.WriteLine(TypeConverter.Convert(null, typeof(TestEnum)));
            //Console.WriteLine(TypeConverter.Convert("XX", typeof(TestEnum)));
            Console.WriteLine(TypeConverter.Convert("One", typeof(TestEnum)));
        }

        enum TestEnum
        {
            One, Two, Three
        }
    }
}
