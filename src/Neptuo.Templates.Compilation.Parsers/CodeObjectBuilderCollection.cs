using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.SyntaxTrees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Collection by node type implementation of <see cref="ICodeObjectBuilder"/>.
    /// Builders are registered by node type.
    /// </summary>
    public class CodeObjectBuilderCollection : ICodeObjectBuilder
    {
        private readonly Dictionary<Type, ICodeObjectBuilder> storage = new Dictionary<Type, ICodeObjectBuilder>();
        private readonly OutFuncCollection<Type, ICodeObjectBuilder, bool> onSearchHandler = new OutFuncCollection<Type, ICodeObjectBuilder, bool>();

        /// <summary>
        /// Adds <paramref name="builder"/> to handler nodes of type <paramref name="nodeType"/>.
        /// </summary>
        /// <param name="nodeType">Type of node to handle by <paramref name="builder"/>.</param>
        /// <param name="builder">Builder to handler nodes of type <paramref name="nodeType"/>.</param>
        /// <returns>Self (for fluency).</returns>
        public CodeObjectBuilderCollection Add(Type nodeType, ICodeObjectBuilder builder)
        {
            Ensure.NotNull(nodeType, "nodeType");
            Ensure.NotNull(builder, "builder");
            storage[nodeType] = builder;
            return this;
        }

        /// <summary>
        /// Adds <paramref name="searchHandler"/> to be executed when no builder is registered for node type.
        /// </summary>
        /// <param name="searchHandler">Search handler for providing builders.</param>
        /// <returns>Self (for fluency).</returns>
        public CodeObjectBuilderCollection AddSearchHandler(OutFunc<Type, ICodeObjectBuilder, bool> searchHandler)
        {
            Ensure.NotNull(searchHandler, "searchHandler");
            onSearchHandler.Add(searchHandler);
            return this;
        }

        public IEnumerable<ICodeObject> TryBuild(ISyntaxNode node, ICodeObjectBuilderContext context)
        {
            Ensure.NotNull(node, "node");
            Ensure.NotNull(context, "context");

            Type nodeType = node.GetType();
            while (nodeType != typeof(object))
            {
                ICodeObjectBuilder builder;
                if (storage.TryGetValue(nodeType, out builder))
                    return builder.TryBuild(node, context);

                if (onSearchHandler.TryExecute(nodeType, out builder))
                    return builder.TryBuild(node, context);

                // Try handle by base type.
                nodeType = nodeType.BaseType;
            }

            return null;
        }
    }
}
