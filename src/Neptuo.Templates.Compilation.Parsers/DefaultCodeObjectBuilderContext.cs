using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class DefaultCodeObjectBuilderContext : ICodeObjectBuilderContext
    {
        public IParserProvider ParserProvider { get; private set; }

        public DefaultCodeObjectBuilderContext(IParserProvider parserProvider)
        {
            Ensure.NotNull(parserProvider, "parserProvider");
            ParserProvider = parserProvider;
        }
    }
}
