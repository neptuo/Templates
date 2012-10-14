using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Neptuo.Web.Framework.Controls;
using Neptuo.Web.Framework.Utils;

namespace Neptuo.Web.Framework.Compilation
{
    public class StandartCodeCompiler
    {
        List<CompilationAssembly> assemblies = new List<CompilationAssembly>();

        public IRegistrator Registrator { get; private set; }
        public string GeneratedCodeFolder { get; set; }

        public StandartCodeCompiler()
        {
            Registrator = new Registrator();

            AddAssembly("System.dll");
            AddAssembly("System.Web.dll");
            AddAssembly(@"C:\Temp\NeptuoFramework\Neptuo.Web.Framework.dll");
        }

        public string ProcessView(string fileName)
        {
            ILivecycleObserver observer = new StandartLivecycleObserver();
            ViewProcessor processor = new ViewProcessor(Registrator, new ServiceProvider(Registrator, observer), observer, GeneratedCodeFolder, assemblies);
            return processor.ProcessView(fileName);
        }

        public void AddAssembly(string assemblyName)
        {
            assemblies.Add(new CompilationAssembly
            {
                AssemblyName = assemblyName
            });
        }

        public void AddAssemblyFile(string assemblyPath)
        {
            assemblies.Add(new CompilationAssembly
            {
                AssemblyPath = assemblyPath
            });
        }
    }

    class ViewProcessor
    {
        List<CompilationAssembly> assemblies;
        IRegistrator registrator;
        IServiceProvider serviceProvider;
        ILivecycleObserver livecycleObserver;
        string generatedCodeFolder;

        public ViewProcessor(IRegistrator registrator, IServiceProvider serviceProvider, ILivecycleObserver livecycleObserver, string generatedCodeFolder, List<CompilationAssembly> assemblies)
        {
            this.registrator = registrator;
            this.serviceProvider = serviceProvider;
            this.livecycleObserver = livecycleObserver;
            this.generatedCodeFolder = generatedCodeFolder;
            this.assemblies = assemblies;
        }

        public string ProcessView(string fileName)
        {
            string assemblyName = GetAssemblyName(fileName);
            if (!IsCompiled(assemblyName))
                CompileView(assemblyName, File.ReadAllText(fileName));

            return RunView(assemblyName);
        }

        private string RunView(string assemblyName)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Assembly views = Assembly.LoadFile(assemblyName);
            Type generatedView = views.GetType(
                String.Format("{0}.{1}", BaseCodeGenerator.Names.CodeNamespace, BaseCodeGenerator.Names.ClassName)
            );

            StringWriter output = new StringWriter();

            IGeneratedView view = (IGeneratedView)Activator.CreateInstance(generatedView);
            view.Setup(new BaseViewPage(livecycleObserver), livecycleObserver, serviceProvider, null, null);
            view.CreateControls();
            view.Init();
            view.Render(new HtmlTextWriter(output));

            stopwatch.Stop();
            Debug.WriteLine("Run in {0}ms", stopwatch.ElapsedMilliseconds);

            return output.ToString();
        }

        private void CompileView(string assemblyName, string viewContent)
        {
            string classFileName = GenerateClass(viewContent);

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters cp = new CompilerParameters();

            //TODO: Zajistit, že temp složka obsahuje všechny assembly
            foreach (CompilationAssembly assembly in assemblies)
                cp.ReferencedAssemblies.Add(assembly.AssemblyName ?? assembly.AssemblyPath);
            
            cp.GenerateExecutable = false;
            cp.OutputAssembly = assemblyName;
            cp.GenerateInMemory = false;
            cp.IncludeDebugInformation = true;

            CompilerResults cr = provider.CompileAssemblyFromFile(cp, classFileName);
            if (cr.Errors.Count > 0)
            {
                // Display compilation errors.
                Debug.WriteLine("Errors building {0}", cr.PathToAssembly);
                foreach (CompilerError ce in cr.Errors)
                {
                    Debug.WriteLine("  {0}", ce.ToString());
                    Debug.WriteLine("");
                }
            }
            else
            {
                Debug.WriteLine("{0} built successfully.", cr.PathToAssembly);
            }
        }

        private string GenerateClass(string viewContent)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Neptuo.Web.Framework.Compilation.CodeGenerator generator = new Neptuo.Web.Framework.Compilation.CodeGenerator();

            var context = new XGeneratorContext
            {
                CodeGenerator = generator,
                ServiceProvider = serviceProvider,
                ParentInfo = new ParentInfo(
                    generator.CreateViewPage(), 
                    TypeHelper.PropertyName<IViewPage>(p => p.Content), 
                    TypeHelper.MethodName<IList<object>, object>(l => l.Add), 
                    typeof(object)
                )
            };

            CodeGeneratorService compiler = new CodeGeneratorService();
            compiler.ContentGenerator = new DefaultContentGenerator(typeof(LiteralControl), TypeHelper.PropertyName<LiteralControl>(l => l.Text), typeof(GenericContentControl), TypeHelper.PropertyName<GenericContentControl>(c => c.TagName));
            compiler.ValueGenerators.Add(new ExtensionValueGenerator());

            compiler.ProcessContent(viewContent, context);

            generator.FinalizeClass();

            using (StreamWriter writer = new StreamWriter("GeneratedView.cs"))
                generator.GenerateCode(writer);

            stopwatch.Stop();
            Debug.WriteLine("Compilation in {0}ms", stopwatch.ElapsedMilliseconds);

            return "GeneratedView.cs";
        }

        private bool IsCompiled(string assemblyName)
        {
            return File.Exists(assemblyName);
        }

        private string GetAssemblyName(string fileName)
        {
            return Path.Combine(generatedCodeFolder, String.Format("View_{0}.dll", HashHelper.Sha1(File.ReadAllText(fileName))));
            //return Path.Combine(generatedCodeFolder, String.Format("View_{0}.dll", fileName.Replace(".html", "")));
        }
    }

    class CompilationAssembly
    {
        public string AssemblyName { get; set; }
        public string AssemblyPath { get; set; }
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

    static class HashHelper
    {
        public static string Sha1(string text)
        {
            SHA1 hasher = SHA1.Create();
            byte[] hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(text));
            StringBuilder result = new StringBuilder();
            foreach (byte hashPart in hash)
                result.Append(hashPart.ToString("X2"));

            return result.ToString();
        }
    }
}
