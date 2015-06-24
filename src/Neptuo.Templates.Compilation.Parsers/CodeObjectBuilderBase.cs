using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.SyntaxTrees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public abstract class CodeObjectBuilderBase<T> : ICodeObjectBuilder
        where T : ISyntaxNode
    {
        public IEnumerable<ICodeObject> TryBuild(ISyntaxNode node, ICodeObjectBuilderContext context)
        {
            return TryBuild((T)node, context);
        }

        protected abstract IEnumerable<ICodeObject> TryBuild(T node, ICodeObjectBuilderContext context);
    }
}
