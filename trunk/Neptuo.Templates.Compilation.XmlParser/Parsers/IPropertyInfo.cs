using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public interface IPropertyInfo
    {
        string Name { get; }
        Type Type { get; }
    }
}
