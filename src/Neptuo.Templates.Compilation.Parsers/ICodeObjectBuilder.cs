using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.SyntaxTrees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public interface ICodeObjectBuilder
    {
        IEnumerable<ICodeObject> TryBuild(ISyntaxNode node, ICodeObjectBuilderContext context);
    }
}
