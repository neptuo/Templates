using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public interface IMarkupExtensionBuilderFactory
    {
        IMarkupExtensionBuilder CreateBuilder(string prefix, string name);
    }
}
