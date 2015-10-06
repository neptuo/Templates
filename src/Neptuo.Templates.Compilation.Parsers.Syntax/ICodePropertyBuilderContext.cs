using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax
{
    public interface ICodePropertyBuilderContext
    {
        /// <summary>
        /// Extensible registry for parsers.
        /// </summary>
        IParserProvider ParserProvider { get; }

        /// <summary>
        /// Name of the property to set.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Type of the property to set.
        /// </summary>
        Type PropertyType { get; }
    }
}
