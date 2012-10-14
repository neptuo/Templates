using Neptuo.Web.Framework;
using Neptuo.Web.Framework.Compilation;
using Neptuo.Web.Framework.Controls;
using Neptuo.Web.Framework.Observers;
using Neptuo.Web.Framework.Utils;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TestConsole
{
    static class TestCompilation
    {
        static ILivecycleObserver livecycleObserver = new StandartLivecycleObserver();
        static IRegistrator registrator = new Registrator();
        static IServiceProvider serviceProvider = new ServiceProvider(registrator, livecycleObserver);

        public static void Test()
        {
            GenerateCode();
        }

        static void GenerateCode()
        {
            registrator.RegisterNamespace("h", "Neptuo.Web.Framework.Controls");
            registrator.RegisterNamespace("h", "Neptuo.Web.Framework.Extensions");
            registrator.RegisterObserver("ui", "visible", typeof(VisibleObserver));

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            XmlContentParser.LiteralTypeDescriptor literal = XmlContentParser.LiteralTypeDescriptor.Create<LiteralControl>(c => c.Text);
            XmlContentParser.GenericContentTypeDescriptor genericContent = XmlContentParser.GenericContentTypeDescriptor.Create<GenericContentControl>(c => c.TagName);

            IParserService parserService = new DefaultParserService();
            parserService.ContentParsers.Add(new XmlContentParser(literal, genericContent));
            parserService.ValueParsers.Add(new ExtensionValueParser());

            ICodeObject rootObject = new BaseParser.RootCodeObject();
            parserService.ProcessContent(File.ReadAllText("index.html"), new DefaultParserServiceContext(serviceProvider, rootObject));

            stopwatch.Stop();
            Console.WriteLine("Compilation in {0}ms", stopwatch.ElapsedMilliseconds);
        }

        #region Compile and run

        static void CompileCode()
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters cp = new CompilerParameters();
            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add("System.Web.dll");
            cp.ReferencedAssemblies.Add("Neptuo.Web.Framework.dll");
            cp.GenerateExecutable = false;
            cp.OutputAssembly = "GeneratedView.dll";
            cp.GenerateInMemory = false;
            cp.IncludeDebugInformation = true;

            CompilerResults cr = provider.CompileAssemblyFromFile(cp, "GeneratedView.cs");
            if (cr.Errors.Count > 0)
            {
                // Display compilation errors.
                Console.WriteLine("Errors building {0}", cr.PathToAssembly);
                foreach (CompilerError ce in cr.Errors)
                {
                    Console.WriteLine("  {0}", ce.ToString());
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("{0} built successfully.", cr.PathToAssembly);
            }
        }

        static void RunCode()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Assembly views = Assembly.Load("GeneratedView");
            Type generatedView = views.GetType("Neptuo.Web.Framework.Generated.GeneratedView");

            StringWriter output = new StringWriter();

            IGeneratedView view = (IGeneratedView)Activator.CreateInstance(generatedView);
            view.Setup(new BaseViewPage(livecycleObserver), livecycleObserver, serviceProvider, null, null);
            view.CreateControls();
            view.Init();
            view.Render(new HtmlTextWriter(output));

            stopwatch.Stop();

            Console.WriteLine(output);
            Console.WriteLine("Run in {0}ms", stopwatch.ElapsedMilliseconds);
        }

        #endregion

        //static void RunStandart()
        //{
        //    StandartCodeCompiler compiler = new StandartCodeCompiler();
        //    compiler.GeneratedCodeFolder = @"C:\Temp\NeptuoFramework";
        //    compiler.Registrator.RegisterNamespace("h", "Neptuo.Web.Framework.Controls");
        //    compiler.Registrator.RegisterNamespace("h", "Neptuo.Web.Framework.Extensions");
        //    compiler.Registrator.RegisterObserver("ui", "visible", typeof(Neptuo.Web.Framework.Observers.VisibleObserver));
        //    compiler.ProcessView("Index.html");
        //}
    }

    class ServiceProvider : IServiceProvider
    {
        private IRegistrator registrator;
        private ILivecycleObserver livecycleObserver;

        public ServiceProvider(IRegistrator registrator, ILivecycleObserver livecycleObserver)
        {
            this.registrator = registrator;
            this.livecycleObserver = livecycleObserver;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IRegistrator))
                return registrator;

            if (serviceType == typeof(ILivecycleObserver))
                return livecycleObserver;

            return null;
        }
    }
}