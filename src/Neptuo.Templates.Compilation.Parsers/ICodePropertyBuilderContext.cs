using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public interface ICodePropertyBuilderContext
    {
        /// <summary>
        /// Extensible registry for parsers.
        /// </summary>
        IParserRegistry Registry { get; }
    }
}
