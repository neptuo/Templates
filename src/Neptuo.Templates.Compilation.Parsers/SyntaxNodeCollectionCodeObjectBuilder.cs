﻿using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.SyntaxTrees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class SyntaxNodeCollectionCodeObjectBuilder : CodeObjectBuilderBase<SyntaxNodeCollection>
    {
        private readonly ICodeObjectBuilder itemBuilder;

        public SyntaxNodeCollectionCodeObjectBuilder(ICodeObjectBuilder itemBuilder)
        {
            Ensure.NotNull(itemBuilder, "itemBuilder");
            this.itemBuilder = itemBuilder;
        }

        protected override IEnumerable<ICodeObject> TryBuild(SyntaxNodeCollection node, ICodeObjectBuilderContext context)
        {
            bool isOk = true;
            CodeObjectList result = new CodeObjectList();
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
