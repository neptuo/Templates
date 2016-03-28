using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax
{
    /// <summary>
    /// Syntax node processor responsible for creating code properties.
    /// </summary>
    public interface ICodePropertyBuilder
    {
        /// <summary>
        /// Creates enumeration of code properties from <paramref name="node"/>.
        /// </summary>
        /// <param name="node">Syntax node to process.</param>
        /// <param name="context">Processing context.</param>
        /// <returns>Enumeration of code objects representing <paramref name="node"/>.</returns>
        IEnumerable<ICodeProperty> TryBuild(INode node, ICodePropertyBuilderContext context);
    }
}
