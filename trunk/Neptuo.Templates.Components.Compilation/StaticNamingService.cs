using Neptuo.Security.Cryptography;
using Neptuo.Templates.Compilation.CodeGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    public class StaticNamingService : INamingService
    {
        protected IFileProvider FileProvider { get; private set; }

        public StaticNamingService(IFileProvider fileProvider)
        {
            FileProvider = fileProvider;
        }

        public INaming FromFile(string fileName)
        {
            return FromContent(FileProvider.GetFileContent(fileName));
        }

        public INaming FromContent(string viewContent)
        {
            return new DefaultNaming(
                CodeDomStructureGenerator.Names.CodeNamespace, 
                CodeDomStructureGenerator.Names.ClassName, 
                GetAssemblyNameForContent(viewContent)
            );
        }

        protected virtual string GetAssemblyNameForContent(string viewContent)
        {
            return String.Format("View_{0}.dll", HashHelper.Sha1(viewContent));
        }
    }
}
