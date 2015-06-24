using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.SyntaxTrees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class CurlyCodeObjectBuilder : CodeObjectBuilderBase<CurlySyntax>
    {
        protected override IEnumerable<ICodeObject> TryBuild(CurlySyntax node, ICodeObjectBuilderContext context)
        {
            throw new NotImplementedException();
        }
    }
}
