//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using Neptuo.Web.Framework.Compilation;
//using Neptuo.Web.Framework.Parser.HtmlContent;
//using Neptuo.Web.Framework;
//using System.Reflection;
//using Neptuo.Web.Framework.Utils;
//using System.Diagnostics;
//using Microsoft.CSharp;
//using System.CodeDom.Compiler;
//using Neptuo.Web.Framework.Observers;
//using Neptuo.Web.Framework.Controls;

//namespace TestConsole
//{
//    class TestCompilerOLD
//    {
//        static ILivecycleObserver livecycleObserver = new StandartLivecycleObserver();
//        static IRegistrator registrator = new Registrator();
//        static IServiceProvider serviceProvider = new ServiceProvider(registrator, livecycleObserver);

//        public static void Test()
//        {
//            //GenerateCode();
//            //CompileCode();
//            //RunCode();
//            RunStandart();
//        }

//        private static void RunStandart()
//        {
//            StandartCodeCompiler compiler = new StandartCodeCompiler();
//            compiler.GeneratedCodeFolder = @"C:\Temp\NeptuoFramework";
//            compiler.Registrator.RegisterNamespace("h", "Neptuo.Web.Framework.Controls");
//            compiler.Registrator.RegisterNamespace("h", "Neptuo.Web.Framework.Extensions");
//            compiler.Registrator.RegisterObserver("ui", "visible", typeof(Neptuo.Web.Framework.Observers.VisibleObserver));
//            compiler.ProcessView("Index.html");
//        }

//        private static void GenerateCode()
//        {
//            Stopwatch stopwatch = new Stopwatch();
//            stopwatch.Start();

//            Neptuo.Web.Framework.Compilation.CodeGenerator generator = new Neptuo.Web.Framework.Compilation.CodeGenerator();
            
//            registrator.RegisterNamespace("h", "Neptuo.Web.Framework.Controls");
//            registrator.RegisterNamespace("h", "Neptuo.Web.Framework.Extensions");
//            registrator.RegisterObserver("ui", "visible", typeof(VisibleObserver));

//            var context = new XGeneratorContext
//            {
//                CodeGenerator = generator,
//                ServiceProvider = serviceProvider,
//                ParentInfo = new ParentInfo(generator.CreateViewPage(), TypeHelper.PropertyName<IViewPage>(p => p.Content), "Add", typeof(object))
//            };

//            CodeGeneratorService compiler = new CodeGeneratorService();
//            compiler.ContentGenerator = new DefaultContentGenerator(typeof(LiteralControl), TypeHelper.PropertyName<LiteralControl>(l => l.Text), typeof(GenericContentControl), TypeHelper.PropertyName<GenericContentControl>(c => c.TagName));
//            compiler.ValueGenerators.Add(new ExtensionValueGenerator());
//            compiler.ProcessContent(File.ReadAllText("Index.html"), context);

//            generator.FinalizeClass();

//            using (StreamWriter writer = new StreamWriter("GeneratedView.cs"))
//                generator.GenerateCode(writer);

//            stopwatch.Stop();
//            Console.WriteLine("Compilation in {0}ms", stopwatch.ElapsedMilliseconds);
//        }

//        private static void CompileCode()
//        {
//            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
//            CompilerParameters cp = new CompilerParameters();
//            cp.ReferencedAssemblies.Add("System.dll");
//            cp.ReferencedAssemblies.Add("System.Web.dll");
//            cp.ReferencedAssemblies.Add("Neptuo.Web.Framework.dll");
//            cp.GenerateExecutable = false;
//            cp.OutputAssembly = "GeneratedView.dll";
//            cp.GenerateInMemory = false;
//            cp.IncludeDebugInformation = true;

//            CompilerResults cr = provider.CompileAssemblyFromFile(cp, "GeneratedView.cs");
//            if (cr.Errors.Count > 0)
//            {
//                // Display compilation errors.
//                Console.WriteLine("Errors building {0}", cr.PathToAssembly);
//                foreach (CompilerError ce in cr.Errors)
//                {
//                    Console.WriteLine("  {0}", ce.ToString());
//                    Console.WriteLine();
//                }
//            }
//            else
//            {
//                Console.WriteLine("{0} built successfully.", cr.PathToAssembly);
//            }
//        }

//        private static void RunCode()
//        {
//            Stopwatch stopwatch = new Stopwatch();
//            stopwatch.Start();

//            Assembly views = Assembly.Load("GeneratedView");
//            Type generatedView = views.GetType("Neptuo.Web.Framework.Generated.GeneratedView");

//            StringWriter output = new StringWriter();

//            IGeneratedView view = (IGeneratedView)Activator.CreateInstance(generatedView);
//            view.Setup(new BaseViewPage(livecycleObserver), livecycleObserver, serviceProvider, null, null);
//            view.CreateControls();
//            view.Init();
//            view.Render(new HtmlTextWriter(output));

//            stopwatch.Stop();

//            Console.WriteLine(output);
//            Console.WriteLine("Run in {0}ms", stopwatch.ElapsedMilliseconds);
//        }
//    }

//    class ServiceProvider : IServiceProvider
//    {
//        private IRegistrator registrator;
//        private ILivecycleObserver livecycleObserver;

//        public ServiceProvider(IRegistrator registrator, ILivecycleObserver livecycleObserver)
//        {
//            this.registrator = registrator;
//            this.livecycleObserver = livecycleObserver;
//        }

//        public object GetService(Type serviceType)
//        {
//            if (serviceType == typeof(IRegistrator))
//                return registrator;

//            if (serviceType == typeof(ILivecycleObserver))
//                return livecycleObserver;

//            return null;
//        }
//    }

//}


//        //private static void GenerateCodeOld()
//        //{
//        //    Stopwatch stopwatch = new Stopwatch();
//        //    stopwatch.Start();

//        //    CodeGenerator generator = new CodeGenerator();
//        //    IRegistrator registrator = new Registrator();
//        //    registrator.RegisterNamespace("h", "Neptuo.Web.Framework.Controls");
//        //    registrator.RegisterNamespace("h", "Neptuo.Web.Framework.Extensions");

//        //    Compiler.RegisterContentCompiler(() => new HtmlContentParser(), () => new ControlContentCompiler());
//        //    Compiler.RegisterContentCompilerContextFactory((parser) =>
//        //    {
//        //        return new ContentCompilerContext
//        //        {
//        //            CodeGenerator = generator,
//        //            CompilerContext = new CompilerContext(),
//        //            ServiceProvider = new ServiceProvider(registrator),
//        //            Parser = parser,
//        //            ParentInfo = new ParentInfo(BaseCodeGenerator.Names.ViewPageField, TypeHelper.PropertyName<IViewPage>(p => p.Content), "Add", typeof(object))
//        //        };
//        //    });
//        //    Compiler.CompileContent(File.ReadAllText("Index.html"), new CompilerContext());

//        //    using (StreamWriter writer = new StreamWriter("GeneratedView.cs"))
//        //        generator.GenerateCode(writer);

//        //    stopwatch.Stop();
//        //    Console.WriteLine("Compilation in {0}ms", stopwatch.ElapsedMilliseconds);
//        //}