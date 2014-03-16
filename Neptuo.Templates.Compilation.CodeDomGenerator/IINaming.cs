using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    public interface INaming
    {
        string SourceName { get; }
        string ClassNamespace { get; }
        string ClassName { get; }
        string AssemblyName { get; }
    }
}
