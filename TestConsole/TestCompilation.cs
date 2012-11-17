using Neptuo.Web.Framework;
using Neptuo.Web.Framework.Compilation;
using Neptuo.Web.Framework.Compilation.CodeGenerators;
using Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions;
using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Compilation.Parsers;
using Neptuo.Web.Framework.Configuration;
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
using System.Threading;

namespace TestConsole
{
    static class TestCompilation
    {
        static IComponentManager componentManager = new ComponentManager();
        static IRegistrator registrator = new Registrator();
        static IServiceProvider serviceProvider = new ServiceProvider(registrator, componentManager);

        static TestCompilation()
        {
            //registrator.RegisterNamespace("h", "Neptuo.Web.Framework.Controls");
            //registrator.RegisterNamespace(null, "Neptuo.Web.Framework.Extensions");
            //registrator.RegisterObserver("ui", "visible", typeof(VisibleObserver));
            //registrator.RegisterObserver("html", null, typeof(HtmlAttributeObserver));
            //registrator.RegisterObserver("val", null, typeof(ValidationObserver));
            //registrator.RegisterObserver("cache", null, typeof(CacheObserver));
            //registrator.RegisterObserver("data", null, typeof(DataContextObserver));
            registrator.LoadSection();
        }

        public static void Test()
        {
            //if (GenerateCode())
            //{
            //    if (CompileCode())
            //    {
            //        RunCode();

            //        //Thread.Sleep(4000);
            //        //RunCode();

            //        //for (int i = 0; i < 10; i++)
            //        //    RunCode();
            //    }
            //}
            RunViewService();
        }

        static bool GenerateCode()
        {
            XmlContentParser.LiteralTypeDescriptor literal = XmlContentParser.LiteralTypeDescriptor.Create<LiteralControl>(c => c.Text);
            XmlContentParser.GenericContentTypeDescriptor genericContent = XmlContentParser.GenericContentTypeDescriptor.Create<GenericContentControl>(c => c.TagName);

            IParserService parserService = new DefaultParserService();
            parserService.ContentParsers.Add(new XmlContentParser(literal, genericContent));
            parserService.ValueParsers.Add(new ExtensionValueParser());

            CodeDomGenerator generator = new CodeDomGenerator();
            generator.SetCodeObjectExtension(typeof(ExtensionCodeObject), new ExtensionCodeDomCodeObjectExtension());

            ICodeGeneratorService generatorService = new DefaultCodeGeneratorService();
            generatorService.AddGenerator("CSharp", generator);




            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            IPropertyDescriptor contentProperty = new ListAddPropertyDescriptor(typeof(BaseViewPage).GetProperty(TypeHelper.PropertyName<BaseViewPage>(v => v.Content)));
            bool parserResult = parserService.ProcessContent(File.ReadAllText("Index.html"), new DefaultParserServiceContext(serviceProvider, contentProperty));
            if (!parserResult)
            {
                Console.WriteLine("Parsing failed!");
                return false;
            }

            using (StreamWriter writer = new StreamWriter("GeneratedView.cs"))
            {
                bool generatorResult = generatorService.GeneratedCode("CSharp", contentProperty, new DefaultCodeGeneratorContext(writer));
                if (!generatorResult)
                {
                    Console.WriteLine("Generating failed!");
                    return false;
                }
            }

            stopwatch.Stop();
            Console.WriteLine("Generated in {0}ms", stopwatch.ElapsedMilliseconds);
            return true;
        }

        static bool CompileCode()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            CodeDomCompiler compiler = new CodeDomCompiler();
            compiler.IncludeDebugInformation = false;
            compiler.AddReferencedFolder(Environment.CurrentDirectory);

            CompilerResults cr = compiler.CompileAssemblyFromFile("GeneratedView.cs");
            if (cr.Errors.Count > 0)
            {
                // Display compilation errors.
                Console.WriteLine("Errors building {0}", cr.PathToAssembly);
                foreach (CompilerError ce in cr.Errors)
                {
                    Console.WriteLine("  {0}", ce.ToString());
                    Console.WriteLine();
                }
                return false;
            }
            else
            {
                Console.WriteLine("{0} built successfully.", cr.PathToAssembly);
            }

            stopwatch.Stop();
            Console.WriteLine("Built in {0}ms", stopwatch.ElapsedMilliseconds);
            return true;
        }

        static void RunCode()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Assembly views = Assembly.Load("GeneratedView");
            Type generatedView = views.GetType("Neptuo.Web.Framework.GeneratedView");

            StringWriter output = new StringWriter();

            IGeneratedView view = (IGeneratedView)Activator.CreateInstance(generatedView);
            view.Setup(new BaseViewPage(componentManager), componentManager, serviceProvider);
            view.CreateControls();
            view.Init();
            view.Render(new HtmlTextWriter(output));

            stopwatch.Stop();

            Console.WriteLine(output);
            Console.WriteLine("Run in {0}ms", stopwatch.ElapsedMilliseconds);
        }

        static void RunStaticCode()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            StringWriter output = new StringWriter();

            IGeneratedView view = new GeneratedView();
            view.Setup(new BaseViewPage(componentManager), componentManager, serviceProvider);
            view.CreateControls();
            view.Init();
            view.Render(new HtmlTextWriter(output));

            stopwatch.Stop();

            Console.WriteLine(output);
            Console.WriteLine("Run in {0}ms", stopwatch.ElapsedMilliseconds);
        }

        static void RunViewService()
        {
            XmlContentParser.LiteralTypeDescriptor literal = XmlContentParser.LiteralTypeDescriptor.Create<LiteralControl>(c => c.Text);
            XmlContentParser.GenericContentTypeDescriptor genericContent = XmlContentParser.GenericContentTypeDescriptor.Create<GenericContentControl>(c => c.TagName);

            CodeDomGenerator generator = new CodeDomGenerator();
            generator.SetCodeObjectExtension(typeof(ExtensionCodeObject), new ExtensionCodeDomCodeObjectExtension());

            CodeDomViewService viewService = new CodeDomViewService(serviceProvider);
            viewService.DebugMode = true;
            viewService.BinDirectory = Environment.CurrentDirectory;
            viewService.TempDirectory = @"C:\Temp\NeptuoFramework";
            viewService.ParserService.ContentParsers.Add(new XmlContentParser(literal, genericContent));
            viewService.ParserService.ValueParsers.Add(new ExtensionValueParser());
            viewService.CodeGeneratorService.AddGenerator("CSharp", generator);


            StringWriter output = new StringWriter();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            IGeneratedView view = ((IViewService)viewService).Process("Index.html");
            view.Setup(new BaseViewPage(componentManager), componentManager, serviceProvider);
            view.CreateControls();
            view.Init();
            view.Render(new HtmlTextWriter(output));

            Console.WriteLine(output);
            Console.WriteLine("Run in {0}ms", stopwatch.ElapsedMilliseconds);
        }
    }

    class ServiceProvider : IServiceProvider
    {
        private IRegistrator registrator;
        private IComponentManager componentManager;

        public ServiceProvider(IRegistrator registrator, IComponentManager componentManager)
        {
            this.registrator = registrator;
            this.componentManager = componentManager;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IRegistrator))
                return registrator;

            if (serviceType == typeof(IComponentManager))
                return componentManager;

            return null;
        }
    }
}