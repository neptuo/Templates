using Neptuo.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    public interface INodeBuilder
    {
        INode Build(TokenReader reader, INodeBuilderContext context);
    }
}
