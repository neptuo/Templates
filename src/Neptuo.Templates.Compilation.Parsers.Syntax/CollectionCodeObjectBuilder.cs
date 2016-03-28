using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax
{
    public class CollectionCodeObjectBuilder : CodeObjectBuilderBase<NodeCollection>
    {
        private readonly ICodeObjectBuilder itemBuilder;

        public CollectionCodeObjectBuilder(ICodeObjectBuilder itemBuilder)
        {
            Ensure.NotNull(itemBuilder, "itemBuilder");
            this.itemBuilder = itemBuilder;
        }

        protected override IEnumerable<ICodeObject> TryBuild(NodeCollection node, ICodeObjectBuilderContext context)
        {
            bool isOk = true;
            CodeObjectCollection result = new CodeObjectCollection();
            foreach (INode item in node.Nodes)
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
