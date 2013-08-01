using Neptuo.Templates;
using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.Parsers;
using Neptuo.Templates.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestConsoleNG.Controls;
using TestConsoleNG.Unity;

namespace TestConsoleNG
{
    public static class TestTemplates
    {
        private static UnityDependencyContainer container;

        static TestTemplates()
        {
            container = new UnityDependencyContainer();
            container.RegisterType<IComponentManager, ComponentManager>();

        }

        public static void Test()
        {
            IBuilderRegistry registry = new TypeBuilderRegistry(
                new LiteralControlBuilder<LiteralControl>(c => c.Text), 
                new GenericContentControlBuilder<GenericContentControl>(c => c.TagName)
            );

            //TODO: Create CodeDomViewService...

            //IParserService parserService = new DefaultParserService();
            //XmlContentParser parser = new XmlContentParser(registry);
            //parserService.ContentParsers.Add(parser);
            //parserService.ProcessContent("<a href='google'>Hello!</a>", new DefaultParserServiceContext(null, null));


            CodeDomViewService viewService = new CodeDomViewService();
            viewService.ParserService.ContentParsers.Add(new XmlContentParser(registry));
            //viewService.ParserService.ValueParsers.Add(new ExtensionValueParser());
            //viewService.CodeDomGenerator.SetCodeObjectExtension(typeof(ExtensionCodeObject), new ExtensionCodeObjectExtension());
            viewService.DebugMode = true;
            viewService.TempDirectory = @"C:\Temp\NeptuoFramework";
            viewService.BinDirectories.Add(Environment.CurrentDirectory);
            CODEDOMREGISTEREXTENSIONS.Register(viewService.CodeDomGenerator);

            //container.RegisterInstance<ContentStorage>(new ContentStorage());
            container.RegisterInstance<IViewService>(viewService);

            StringWriter output = new StringWriter();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //try
            //{

            //IWebGeneratedView view = (IWebGeneratedView)((IViewService)viewService).Process("Index.html", new DefaultViewServiceContext(container));
            BaseGeneratedView view = (BaseGeneratedView)((IViewService)viewService).ProcessContent("<a href='google'>Hello, World!</a>", new DefaultViewServiceContext(container));
            DebugUtils.Run("Run", () =>
            {
                view.Setup(new BaseViewPage(container.Resolve<IComponentManager>()), container.Resolve<IComponentManager>(), container);
                view.CreateControls();
                view.Init();
                view.Render(new HtmlTextWriter(output));
                view.Dispose();
            });

            stopwatch.Stop();
            //}
            //catch (CodeDomViewServiceException e)
            //{
            //    output.WriteLine("Errors occured!");
            //    output.WriteLine();
            //    foreach (IErrorInfo error in e.Errors)
            //    {
            //        output.WriteLine("Line: {0}", error.Line);
            //        output.WriteLine("Position: {0}", error.Column);
            //        output.WriteLine("Message: {0}", error.ErrorText);
            //        output.WriteLine("Error number: {0}", error.ErrorNumber);
            //        output.WriteLine();
            //    }
            //}

            Console.WriteLine(output);
            Console.WriteLine("Run in {0}ms, size {1}ch", stopwatch.ElapsedMilliseconds, output.ToString().Length);
        }
    }
}
