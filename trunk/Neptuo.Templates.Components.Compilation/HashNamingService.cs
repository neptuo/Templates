using Neptuo.Security.Cryptography;
using Neptuo.Templates.Compilation.CodeGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    public class HashNamingService : INamingService
    {
        public const string ViewNameFormat = "View_{0}";

        protected IFileProvider FileProvider { get; private set; }

        public HashNamingService(IFileProvider fileProvider)
        {
            FileProvider = fileProvider;
        }

        public INaming FromFile(string fileName)
        {
            return GetNaming(GetNameForFile(fileName));
        }

        public INaming FromContent(string viewContent)
        {
            return GetNaming(GetNameForContent(viewContent));
        }

        protected virtual INaming GetNaming(string viewName)
        {
            return new DefaultNaming(
                CodeDomStructureGenerator.Names.CodeNamespace,
                viewName,
                GetAssemblyForName(viewName)
            );
        }

        protected virtual string GetNameForFile(string fileName)
        {
            return String.Format(ViewNameFormat, HashHelper.Sha1(FileProvider.GetFileContent(fileName)));
        }

        protected virtual string GetNameForContent(string viewContent)
        {
            return String.Format(ViewNameFormat, HashHelper.Sha1(viewContent));
        }

        protected virtual string GetAssemblyForName(string viewName)
        {
            return String.Format("{0}.dll", viewName);
        }
    }
}
