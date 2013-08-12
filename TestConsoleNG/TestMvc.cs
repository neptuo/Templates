using Neptuo.Templates;
using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TestConsoleNG.Unity;

namespace TestConsoleNG
{
    class TestMvc
    {
        private static UnityDependencyContainer container;

        static TestMvc()
        {
            container = new UnityDependencyContainer();
            container.RegisterInstance<IFileProvider>(new FileProvider(new CurrentDirectoryVirtualPathProvider()));
        }

        public static void Test()
        {


            MvcViewService viewService = new MvcViewService();
            viewService.ParserService.ContentParsers.Add(new XmlContentParser(null));//TODO: Insert registry
            viewService.ParserService.DefaultValueParser = new PlainValueParser();
            viewService.NamingService = new HashNamingService(new FileProvider(new CurrentDirectoryVirtualPathProvider()));
            viewService.DebugMode = true;
            viewService.TempDirectory = @"C:\Temp\NeptuoFramework2";
            viewService.BinDirectories.Add(Environment.CurrentDirectory);



            StringWriter output = new StringWriter();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //try
            //{

            IViewServiceContext context = new ViewServiceContext(container);

            //BaseGeneratedView view = (BaseGeneratedView)viewService.ProcessContent("<h:panel class='checkin'><a href='google'>Hello, World!</a></h:panel>", context);
            WebViewPage<PersonModel> view = (WebViewPage<PersonModel>)viewService.Process("IndexMvc.html", context);
            DebugUtils.Run("Run", () =>
            {
                view.ExecutePageHierarchy();
            });

            stopwatch.Stop();
        }
    }
}
