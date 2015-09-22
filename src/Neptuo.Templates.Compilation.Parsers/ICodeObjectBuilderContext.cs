using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public interface ICodeObjectBuilderContext
    {
        /// <summary>
        /// Extensible registry for parsers.
        /// </summary>
        IParserProvider Registry { get; }
    }
}
