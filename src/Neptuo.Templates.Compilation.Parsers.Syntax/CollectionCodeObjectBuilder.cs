using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class CollectionCodeObjectBuilder : CodeObjectBuilderBase<SyntaxNodeCollection>
    {
        private readonly ICodeObjectBuilder itemBuilder;

        public CollectionCodeObjectBuilder(ICodeObjectBuilder itemBuilder)
        {
            Ensure.NotNull(itemBuilder, "itemBuilder");
            this.itemBuilder = itemBuilder;
        }

        protected override IEnumerable<ICodeObject> TryBuild(SyntaxNodeCollection node, ICodeObjectBuilderContext context)
        {
            bool isOk = true;
            CodeObjectCollection result = new CodeObjectCollection();
            foreach (ISyntaxNode item in node.Nodes)
            {
                IEnumerable<ICodeObject> items = itemBuilder.TryBuild(item, context);
                if (items == null)
                    isOk = false;
                else
                    result.AddRange(items);
            }

            if (isOk)
                return result;

            return null;
        }
    }
}
