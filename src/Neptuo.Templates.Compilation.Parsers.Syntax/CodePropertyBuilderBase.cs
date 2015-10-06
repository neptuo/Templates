using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax
{
    public abstract class CodePropertyBuilderBase<T> : ICodePropertyBuilder
        where T : ISyntaxNode
    {
        public IEnumerable<ICodeProperty> TryBuild(ISyntaxNode node, ICodePropertyBuilderContext context)
        {
            return TryBuild((T)node, context);
        }

        protected abstract IEnumerable<ICodeProperty> TryBuild(T node, ICodePropertyBuilderContext context);

        protected IEnumerable<ICodeObject> Result(params ICodeObject[] codeObjects)
        {
            return new CodeObjectCollection().AddRange(codeObjects);
        }
    }
}
