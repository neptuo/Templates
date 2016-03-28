using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax
{
    public class TextCodeObjectBuilder : CodeObjectBuilderBase<LiteralNode>
    {
        protected override IEnumerable<ICodeObject> TryBuild(LiteralNode node, ICodeObjectBuilderContext context)
        {
            return new CodeObjectCollection().AddPlainValue(node.TextToken.Text);
        }
    }
}
