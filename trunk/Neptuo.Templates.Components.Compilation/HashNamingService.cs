using Neptuo.Security.Cryptography;
using Neptuo.Templates.Compilation.CodeGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// NamingService using <see cref="HashHelper.Sha1"/>
    /// </summary>
    public class HashNamingService : NamingServiceBase
    {
        /// <summary>
        /// Access to file system.
        /// </summary>
        protected IFileProvider FileProvider { get; private set; }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="fileProvider">Access to file system.</param>
        public HashNamingService(IFileProvider fileProvider)
        {
            Guard.NotNull(fileProvider, "fileProvider");
            FileProvider = fileProvider;
        }

        /// <summary>
        /// Returns generated view name for template file <paramref name="fileName"/>
        /// </summary>
        /// <param name="fileName">Template file name.</param>
        /// <returns>Generated view name for template file <paramref name="fileName"/></returns>
        protected override string GetNameForFile(string fileName)
        {
            Guard.NotNull(fileName, "fileName");
            return String.Format(ViewNameFormat, HashHelper.Sha1(FileProvider.GetFileContent(fileName)));
        }

        /// <summary>
        /// Returns generated view name for template content <paramref name="viewContent"/>
        /// </summary>
        /// <param name="viewContent">Template content.</param>
        /// <returns>Generated view name for template content <paramref name="viewContent"/></returns>
        protected override string GetNameForContent(string viewContent)
        {
            Guard.NotNull(viewContent, "viewContent");
            return String.Format(ViewNameFormat, HashHelper.Sha1(viewContent));
        }
    }
}
