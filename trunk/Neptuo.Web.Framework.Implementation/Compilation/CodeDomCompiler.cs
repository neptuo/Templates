using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public class CodeDomCompiler
    {
        CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
        CompilerParameters compilerParameters = new CompilerParameters
        {
            GenerateExecutable = false,
            IncludeDebugInformation = false
        };

        public bool GenerateInMemory
        {
            get { return compilerParameters.GenerateInMemory; }
            set { compilerParameters.GenerateInMemory = value; }
        }

        public bool IncludeDebugInformation
        {
            get { return compilerParameters.IncludeDebugInformation; }
            set { compilerParameters.IncludeDebugInformation = value; }
        }

        public void AddReferencedAssemblies(params string[] path)
        {
            foreach (string item in path)
                compilerParameters.ReferencedAssemblies.Add(item);
        }

        public void AddReferencedFolder(string path)
        {
            AddReferencedAssemblies(Directory.GetFiles(path, "*.dll"));
        }

        public CompilerResults CompileAssemblyFromFile(string fileName, string output = null)
        {
            if (output == null && !compilerParameters.GenerateInMemory)
            {
                if (fileName.EndsWith(".cs"))
                    output = fileName.Replace(".cs", ".dll");
            }

            compilerParameters.OutputAssembly = output;
            return provider.CompileAssemblyFromFile(compilerParameters, fileName);
        }

        public CompilerResults CompileAssemblyFromSource(string source, string output = null)
        {
            compilerParameters.GenerateExecutable = output == null;
            compilerParameters.OutputAssembly = output;
            return provider.CompileAssemblyFromSource(compilerParameters, source);
        }
    }
}
