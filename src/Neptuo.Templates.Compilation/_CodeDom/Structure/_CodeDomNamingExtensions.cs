using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public static class _CodeDomNamingExtensions
    {
        public static string FormatFileName(this ICodeDomNaming naming, string fileExtension)
        {
            Guard.NotNull(naming, "naming");
            return String.Format("{0}.{1}", naming.ClassName, fileExtension);
        }
        #region AssemblyFileName

        public static CodeDomDefaultNaming AddAssemblyFileName(this CodeDomDefaultNaming naming, string assemblyFileName)
        {
            Guard.NotNull(naming, "naming");
            return naming.AddCustomValue("AssemblyFileName", assemblyFileName);
        }

        public static bool TryGetAssemblyFileName(this ICodeDomNaming naming, out string assemblyFileName)
        {
            return naming.CustomValues.TryGet("AssemblyFileName", out assemblyFileName);
        }

        public static string AssemblyName(this ICodeDomNaming naming)
        {
            Guard.NotNull(naming, "naming");

            string fileName;
            if (!TryGetAssemblyFileName(naming, out fileName))
                fileName = FormatFileName(naming, "cs");

            return String.Format("{0}.dll", naming.ClassName);
        }

        #endregion

        #region SourceFileName

        public static CodeDomDefaultNaming AddSourceFileName(this CodeDomDefaultNaming naming, string sourceFileName)
        {
            Guard.NotNull(naming, "naming");
            return naming.AddCustomValue("SourceFileName", sourceFileName);
        }

        public static bool TryGetSourceFileName(this ICodeDomNaming naming, out string sourceFileName)
        {
            return naming.CustomValues.TryGet("SourceFileName", out sourceFileName);
        }

        public static string SourceFileName(this ICodeDomNaming naming)
        {
            Guard.NotNull(naming, "naming");

            string fileName;
            if (!TryGetSourceFileName(naming, out fileName))
                fileName = FormatFileName(naming, "cs");

            return fileName;
        }

        #endregion
    }
}
