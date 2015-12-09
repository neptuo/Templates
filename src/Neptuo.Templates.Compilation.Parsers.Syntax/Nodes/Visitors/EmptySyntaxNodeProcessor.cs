using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors
{
    public class EmptySyntaxNodeProcessor : ISyntaxNodeProcessor
    {
        public static readonly EmptySyntaxNodeProcessor Instance = new EmptySyntaxNodeProcessor();

        public void Process(ISyntaxNode node, ISyntaxNodeProcessor nextProcessor)
        { }
    }
}
