
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

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
            TestCompiler.Test();
            //TestConverter.Test();

            //TestExtensionParser.Test();
            //TestReflection.Test();
            //TestReflection.Test3();
            //TestEvaluator.Test();
            //TestLocalization.Test();
            //TestLambda.Test();

            Console.ReadKey(true);
        }
    }
}
