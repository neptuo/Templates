using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    public class DefaultNaming : INaming
    {
        public string SourceName { get; set; }
        public string ClassNamespace { get; set; }
        public string ClassName { get; set; }
        public string AssemblyName { get; set; }

        public DefaultNaming(string sourceName, string classNamespace, string className, string assemblyName)
        {
            SourceName = sourceName;
            ClassNamespace = classNamespace;
            ClassName = className;
            AssemblyName = assemblyName;
        }
    }
}
