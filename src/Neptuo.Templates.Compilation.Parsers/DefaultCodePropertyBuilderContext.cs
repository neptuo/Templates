using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class DefaultCodePropertyBuilderContext : ICodePropertyBuilderContext
    {
        public IParserProvider ParserProvider { get; private set; }

        public DefaultCodePropertyBuilderContext(IParserProvider parserProvider)
        {
            Ensure.NotNull(parserProvider, "parserProvider");
            ParserProvider = parserProvider;
        }
    }
}
