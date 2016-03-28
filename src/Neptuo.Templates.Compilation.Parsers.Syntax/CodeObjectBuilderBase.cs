using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax
{
    public abstract class CodeObjectBuilderBase<T> : ICodeObjectBuilder
        where T : INode
    {
        public IEnumerable<ICodeObject> TryBuild(INode node, ICodeObjectBuilderContext context)
        {
            return TryBuild((T)node, context);
        }

        protected abstract IEnumerable<ICodeObject> TryBuild(T node, ICodeObjectBuilderContext context);

        protected IEnumerable<ICodeObject> Result(params ICodeObject[] codeObjects)
        {
            return new CodeObjectCollection().AddRange(codeObjects);
        }
    }
}
