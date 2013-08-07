using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    public interface IMarkupExtensionBuilderRegistry
    {
        IMarkupExtensionBuilder GetMarkupExtensionBuilder(string prefix, string name);
    }
}
