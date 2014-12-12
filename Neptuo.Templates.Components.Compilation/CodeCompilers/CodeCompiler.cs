using Neptuo.Compilers;
using Neptuo.Reflection;
using Neptuo.Templates.Compilation.ViewActivators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeCompilers
{
    /// <summary>
    /// Default code compiler for components.
    /// </summary>
    public class CodeCompiler : StaticCodeCompilerBase, IViewActivator
    {
        private readonly string tempDirectory;

        public CodeCompiler(string tempDirectory)
            : base(tempDirectory)
        {
            this.tempDirectory = tempDirectory;
        }

        protected override object Compile(IStaticCompiler compiler, string tempDirectory, TextReader sourceCode, ICodeCompilerContext context)
        {
            INaming naming = context.DependencyProvider.Resolve<INaming>();
            string assemblyFile = Path.Combine(tempDirectory, naming.AssemblyName);
            string sourceFile = Path.Combine(tempDirectory, naming.AssemblyName.Replace(".dll", ".cs"));
            string sourceContent = sourceCode.ReadToEnd();
            string typeFullName = String.Format("{0}.{1}", naming.ClassNamespace, naming.ClassName);
            
            if (IsDebugMode)
                File.WriteAllText(sourceFile, sourceContent);

            ICompilerResult result = compiler.FromSourceCode(sourceContent, assemblyFile);
            if (!result.IsSuccess)
            {
                context.Errors.AddRange(result.Errors);
                return null;
            }

            return Activate(assemblyFile, typeFullName);
        }

        public object Activate(ISourceContent content, IViewActivatorContext context)
        {
            INaming naming = context.DependencyProvider.Resolve<INaming>();
            string assemblyFile = Path.Combine(tempDirectory, naming.AssemblyName);
            string typeFullName = String.Format("{0}.{1}", naming.ClassNamespace, naming.ClassName);

            return Activate(assemblyFile, typeFullName);
        }

        protected object Activate(string assemblyFile, string typeFullName)
        {
            if (!File.Exists(assemblyFile))
                return null;

            Assembly assembly = ReflectionFactory.FromCurrentAppDomain().LoadAssembly(assemblyFile);
            Type generatedType = assembly.GetType(typeFullName);

            object instance = Activator.CreateInstance(generatedType);
            return instance;
        }
    }
}
