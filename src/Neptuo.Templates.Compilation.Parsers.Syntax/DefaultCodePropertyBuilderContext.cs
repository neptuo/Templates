using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax
{
    /// <summary>
    /// Default implementation of <see cref="ICodePropertyBuilderContext"/>.
    /// </summary>
    public class DefaultCodePropertyBuilderContext : ICodePropertyBuilderContext
    {
        public IParserProvider ParserProvider { get; private set; }
        public string PropertyName {get; private set;}
        public Type PropertyType { get; private set; }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="parserProvider">Extensible registry for parsers.</param>
        /// <param name="propertyName">Name of the property to set.</param>
        /// <param name="propertyType">Type of the property to set.</param>
        public DefaultCodePropertyBuilderContext(IParserProvider parserProvider, string propertyName, Type propertyType)
        {
            Ensure.NotNull(parserProvider, "parserProvider");
            Ensure.NotNull(propertyName, "propertyName");
            Ensure.NotNull(propertyType, "propertyType");
            ParserProvider = parserProvider;
            PropertyName = propertyName;
            PropertyType = propertyType;
        }
    }
}
