using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Compilation;
using Neptuo.Web.Framework.Parser.HtmlContent;
using Neptuo.Web.Framework;
using System.Reflection;

namespace TestConsole
{
    class TestCompiler
    {
        public static void Test()
        {
            CodeGenerator generator = new CodeGenerator();
            IRegistrator registrator = new Registrator();
            registrator.RegisterNamespace("h", "Neptuo.Web.Framework.Controls");

            Compiler.RegisterContentCompiler(() => new HtmlContentParser(), () => new ControlContentCompiler());
            Compiler.RegisterContentCompilerContextFactory((parser) =>
            {
                return new ContentCompilerContext
                {
                    CodeGenerator = generator,
                    CompilerContext = new CompilerContext(),
                    ServiceProvider = new ServiceProvider(registrator),
                    Parser = parser
                };
            });
            Compiler.CompileContent(File.ReadAllText("Index.html"), new CompilerContext());

            using (StreamWriter writer = new StreamWriter("GeneratedView.cs"))
            {
                generator.GenerateCode(writer);
            }

            Assembly views = Assembly.Load("GeneratedView");
            Type generatedView = views.GetType("Neptuo.Web.Framework.Generated.GeneratedView");

            IGeneratedView view = (IGeneratedView)Activator.CreateInstance(generatedView);
            view.ViewPage = new BaseViewPage();
            view.CreateControls();
            view.Init();
            view.Render(new HtmlTextWriter(Console.Out));
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
