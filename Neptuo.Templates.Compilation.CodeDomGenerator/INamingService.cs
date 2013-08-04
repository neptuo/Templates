using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    public interface INamingService
    {
        INaming FromFile(string fileName);
        INaming FromContent(string viewContent);
    }

    public interface INaming
    {
        string ClassNamespace { get; }
        string ClassName { get; }
        string AssemblyName { get; }
    }

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
