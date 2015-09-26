using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.SyntaxTrees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class TextCodeObjectBuilder : CodeObjectBuilderBase<TextSyntax>
    {
        protected override IEnumerable<ICodeObject> TryBuild(TextSyntax node, ICodeObjectBuilderContext context)
        {
            return new CodeObjectList().AddPlainValue(node.TextToken.Text);
        }
    }
}
