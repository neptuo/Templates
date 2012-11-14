
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //Assembly assembly = Assembly.Load("Neptuo.Web.Parser");
            //Assembly assembly = Assembly.Load("Neptuo.Web.Parser, Version=1.0.0.0, Culture=neutral");

            //IEvaluator evaluator = (IEvaluator)Activator.CreateInstance(LoadType("Neptuo.Web.Mvc.ViewEngine.HtmlEvaluator, Neptuo.Web.Mvc"));

            //TestParser.Test();
            TestCompilation.Test();
            //TestConverter.Test();
            //TestXml.Test();

            //TestExtensionParser.Test();
            //TestReflection.Test();
            //TestReflection.Test3();
            //TestEvaluator.Test();
            //TestLocalization.Test();
            //TestLambda.Test();

            //PropertyInfo property = typeof(X).GetProperty("Getter");

            //TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(TimeSpan?));
            //Console.WriteLine(typeConverter.CanConvertFrom(typeof(String)));
            //TimeSpan timeSpan = (TimeSpan)typeConverter.ConvertFrom("0:0:5.1");
            //Console.WriteLine(timeSpan);

            Console.ReadKey(true);
        }
    }

    class X
    {
        public Func<int, int> Getter { get; set; }
    }
}
