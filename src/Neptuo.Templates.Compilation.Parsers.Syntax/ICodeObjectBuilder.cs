using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Syntax node processor responsible for creating code objects.
    /// </summary>
    public interface ICodeObjectBuilder
    {
        /// <summary>
        /// Creates enumeration of code objects from <paramref name="node"/>.
        /// </summary>
        /// <param name="node">Syntax node to process.</param>
        /// <param name="context">Processing context.</param>
        /// <returns>Enumeration of code objects representing <paramref name="node"/>.</returns>
        IEnumerable<ICodeObject> TryBuild(ISyntaxNode node, ICodeObjectBuilderContext context);
    }
}
