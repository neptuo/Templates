using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors
{
    /// <summary>
    /// Common extensions for <see cref="ISyntaxNodeProcessor"/>.
    /// </summary>
    public static class _SyntaxNodeProcessorExtensions
    {
        public static void Process(this ISyntaxNodeProcessor processor, ISyntaxNode node)
        {
            Ensure.NotNull(processor, "processor");
            processor.Process(node, EmptySyntaxNodeProcessor.Instance);
        }
    }
}
