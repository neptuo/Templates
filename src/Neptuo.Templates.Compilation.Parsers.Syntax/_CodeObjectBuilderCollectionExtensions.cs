using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax
{
    /// <summary>
    /// Common extensions for <see cref="CodeObjectBuilderCollection"/>.
    /// </summary>
    public static class _CodeObjectBuilderCollectionExtensions
    {
        /// <summary>
        /// Adds <paramref name="builder"/> to handler nodes of type <typeparamref name="TNode"/>.
        /// </summary>
        /// <typeparam name="TNode">Type of node to handle by <paramref name="builder"/></typeparam>
        /// <param name="collection">Target collection to extend.</param>
        /// <param name="builder">Builder to handler nodes of type <typeparamref name="TNode"/>.</param>
        /// <returns>Self (for fluency).</returns>
        public static CodeObjectBuilderCollection Add<TNode>(this CodeObjectBuilderCollection collection, ICodeObjectBuilder builder)
            where TNode : INode
        {
            Ensure.NotNull(collection, "collection");
            return collection.Add(typeof(TNode), builder);
        }
    }
}
