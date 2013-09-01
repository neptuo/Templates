using Neptuo.Security.Cryptography;
using Neptuo.Templates.Compilation.CodeGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    public class HashNamingService : BaseNamingService
    {
        protected IFileProvider FileProvider { get; private set; }

        public HashNamingService(IFileProvider fileProvider)
        {
            FileProvider = fileProvider;
        }

        protected override string GetNameForFile(string fileName)
        {
            return String.Format(ViewNameFormat, HashHelper.Sha1(FileProvider.GetFileContent(fileName)));
        }

        protected override string GetNameForContent(string viewContent)
        {
            return String.Format(ViewNameFormat, HashHelper.Sha1(viewContent));
        }
    }
}
