using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors
{
    public interface ISyntaxNodeProcessor
    {
        void Process(ISyntaxNode node, ISyntaxNodeProcessor nextProcessor);
    }
}
