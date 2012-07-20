using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Compilation;
using Neptuo.Web.Framework.Parser.HtmlContent;
using Neptuo.Web.Framework;
using System.Reflection;
using Neptuo.Web.Framework.Utils;
using System.Diagnostics;

namespace TestConsole
{
    class TestCompiler
    {
        public static void Test()
        {
            GenerateCode();
            RunCode();
        }

        private static void GenerateCode()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            CodeGenerator generator = new CodeGenerator();
            IRegistrator registrator = new Registrator();
            registrator.RegisterNamespace("h", "Neptuo.Web.Framework.Controls");
            registrator.RegisterNamespace("h", "Neptuo.Web.Framework.Extensions");

            Compiler.RegisterContentCompiler(() => new HtmlContentParser(), () => new ControlContentCompiler());
            Compiler.RegisterContentCompilerContextFactory((parser) =>
            {
                return new ContentCompilerContext
                {
                    CodeGenerator = generator,
                    CompilerContext = new CompilerContext(),
                    ServiceProvider = new ServiceProvider(registrator),
                    Parser = parser,
                    ParentInfo = new ParentInfo(BaseCodeGenerator.GeneratedViewPageField, TypeHelper.PropertyName<IViewPage>(p => p.Content), "Add", typeof(object))
                };
            });
            Compiler.CompileContent(File.ReadAllText("Index.html"), new CompilerContext());

            using (StreamWriter writer = new StreamWriter("GeneratedView.cs"))
                generator.GenerateCode(writer);

            stopwatch.Stop();
            Console.WriteLine("Compilation in {0}ms", stopwatch.ElapsedMilliseconds);
        }

        private static void RunCode()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Assembly views = Assembly.Load("GeneratedView");
            Type generatedView = views.GetType("Neptuo.Web.Framework.Generated.GeneratedView");

            StringWriter output = new StringWriter();

            IGeneratedView view = (IGeneratedView)Activator.CreateInstance(generatedView);
            view.ViewPage = new BaseViewPage();
            view.CreateControls();
            view.Init();
            view.Render(new HtmlTextWriter(output));

            stopwatch.Stop();

            Console.WriteLine(output);
            Console.WriteLine("Run in {0}ms", stopwatch.ElapsedMilliseconds);
        }
    }

    class ServiceProvider : IServiceProvider
    {
        private IRegistrator registrator;

        public ServiceProvider(IRegistrator registrator)
        {
            this.registrator = registrator;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IRegistrator))
                return registrator;

            return null;
        }
    }

}
