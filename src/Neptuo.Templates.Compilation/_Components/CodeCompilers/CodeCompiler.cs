using Neptuo.Activators;
using Neptuo.Compilers;
using Neptuo.Reflection;
using Neptuo.Templates.Compilation.CodeGenerators;
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
        protected override object Compile(IStaticCompiler compiler, TextReader sourceCode, ICodeCompilerContext context)
        {
            ICodeDomNaming naming = context.DependencyProvider.Resolve<ICodeDomNaming>();
            string assemblyFile = Path.Combine(this.TempDirectory(), naming.ClassName);
            string sourceFile = Path.Combine(this.TempDirectory(), naming.SourceFileName());
            string sourceContent = sourceCode.ReadToEnd();
            string typeFullName = naming.FullClassName;
            
            if (this.IsDebugMode())
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
            ICodeDomNaming naming = context.DependencyProvider.Resolve<ICodeDomNaming>();
            string assemblyFile = Path.Combine(this.TempDirectory(), naming.AssemblyName());
            string typeFullName = naming.FullClassName;

            return Activate(assemblyFile, typeFullName);
        }

        protected object Activate(string assemblyFile, string typeFullName)
        {
            if (!File.Exists(assemblyFile))
                return null;

            Assembly assembly = ReflectionFactory.FromCurrentAppDomain().LoadAssembly(assemblyFile);
            Type generatedType = assembly.GetType(typeFullName);

            if (generatedType == null)
                return null;

            object instance = Activator.CreateInstance(generatedType);
            return instance;
        }
    }
}
