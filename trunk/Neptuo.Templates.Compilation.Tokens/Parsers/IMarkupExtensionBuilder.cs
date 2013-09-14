using Neptuo.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public interface IMarkupExtensionBuilder
    {
        bool Parse(IMarkupExtensionBuilderContext context, Token extension);
    }
}
