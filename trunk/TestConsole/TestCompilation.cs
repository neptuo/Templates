using Neptuo.Web.Framework;
using Neptuo.Web.Framework.Compilation;
using Neptuo.Web.Framework.Compilation.CodeObjects;
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
using ICodeGenerator = Neptuo.Web.Framework.Compilation.ICodeGenerator;

namespace TestConsole
{
    static class TestCompilation
    {
        static IComponentManager componentManager = new StandartComponentManager();
        static IRegistrator registrator = new Registrator();
        static IServiceProvider serviceProvider = new ServiceProvider(registrator, componentManager);

        public static void Test()
        {
            if (GenerateCode())
            {
                if (CompileCode())
                {
                    RunCode();
                }
            }
            //RunStaticCode();
        }

        static bool GenerateCode()
        {
            registrator.RegisterNamespace("h", "Neptuo.Web.Framework.Controls");
            registrator.RegisterNamespace("h", "Neptuo.Web.Framework.Extensions");
            registrator.RegisterObserver("ui", "visible", typeof(VisibleObserver));
            registrator.RegisterObserver("val", "max-length", typeof(ValidationObserver));
            registrator.RegisterObserver("val", "min-length", typeof(ValidationObserver));
            registrator.RegisterObserver("val", "regex", typeof(ValidationObserver));
            registrator.RegisterObserver("val", "message", typeof(ValidationObserver));

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            XmlContentParser.LiteralTypeDescriptor literal = XmlContentParser.LiteralTypeDescriptor.Create<LiteralControl>(c => c.Text);
            XmlContentParser.GenericContentTypeDescriptor genericContent = XmlContentParser.GenericContentTypeDescriptor.Create<GenericContentControl>(c => c.TagName);

            IParserService parserService = new DefaultParserService();
            parserService.ContentParsers.Add(new XmlContentParser(literal, genericContent));
            parserService.ValueParsers.Add(new ExtensionValueParser());


            IPropertyDescriptor contentProperty = new ListAddPropertyDescriptor(typeof(BaseViewPage).GetProperty(TypeHelper.PropertyName<BaseViewPage>(v => v.Content)));
            bool parserResult = parserService.ProcessContent(File.ReadAllText("Index.html"), new DefaultParserServiceContext(serviceProvider, contentProperty));
            if (!parserResult)
            {
                Console.WriteLine("Parsing failed!");
                return false;
            }

            using (StreamWriter writer = new StreamWriter("GeneratedView.cs"))
            {
                CodeDomGenerator generator = new CodeDomGenerator();
                //generator.AddExtension<ExtensionCodeObject, CodeDomGenerator>();
                bool generatorResult = generator.ProcessTree(contentProperty, new DefaultCodeGeneratorContext(writer));
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

        #region Compile and run

        static bool CompileCode()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters cp = new CompilerParameters();
            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add("System.Web.dll");
            cp.ReferencedAssemblies.Add("Neptuo.Web.Framework.dll");
            cp.ReferencedAssemblies.Add("Neptuo.Web.Framework.Implementation.dll");
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
            view.Setup(new BaseViewPage(componentManager), componentManager, serviceProvider, null, null);
            view.CreateControls();
            view.Init();
            view.Render(new HtmlTextWriter(output));

            stopwatch.Stop();

            Console.WriteLine(output);
            Console.WriteLine("Run in {0}ms", stopwatch.ElapsedMilliseconds);
        }

        #endregion

        static void RunStaticCode()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            StringWriter output = new StringWriter();

            IGeneratedView view = new GeneratedView();
            view.Setup(new BaseViewPage(componentManager), componentManager, serviceProvider, null, null);
            view.CreateControls();
            view.Init();
            view.Render(new HtmlTextWriter(output));

            stopwatch.Stop();

            Console.WriteLine(output);
            Console.WriteLine("Run in {0}ms", stopwatch.ElapsedMilliseconds);
        }

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