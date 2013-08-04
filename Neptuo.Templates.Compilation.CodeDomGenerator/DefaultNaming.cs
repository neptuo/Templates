using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    public class DefaultNaming : INaming
    {
        public string ClassNamespace { get; set; }
        public string ClassName { get; set; }
        public string AssemblyName { get; set; }

        public DefaultNaming(string classNamespace, string className, string assemblyName)
        {
            ClassNamespace = classNamespace;
            ClassName = className;
            AssemblyName = assemblyName;
        }
    }
}
