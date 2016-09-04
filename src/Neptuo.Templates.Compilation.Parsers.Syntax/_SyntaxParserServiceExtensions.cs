using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using Neptuo.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax
{
    /// <summary>
    /// Common extensions for <see cref="SyntaxParserService"/>.
    /// </summary>
    public static class _SyntaxParserServiceExtensions
    {
        /// <summary>
        /// Adds <paramref name="builder"/> to build syntax nodes of token <paramref name="type"/>.
        /// </summary>
        /// <param name="service">The service to add node builder to.</param>
        /// <param name="type">The type of token to be processed by <paramref name="builder"/>.</param>
        /// <param name="builder">The syntax node builder to add.</param>
        /// <returns><paramref name="service"/>.</returns>
        public static SyntaxParserService AddSyntaxNodeBuilder(this SyntaxParserService service, TokenType type, INodeBuilder builder)
        {
            Ensure.NotNull(service, "service");
            service.NodeBuilders.Add(type, builder);
            return service;
        }
    }
}
