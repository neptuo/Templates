using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions.Sources
{
    /// <summary>
    /// The provider of registered namespaces of curly components.
    /// </summary>
    public interface ICurlyCompletionSource
    {
        /// <summary>
        /// Returns an enumeration of completions for curly components based on current <paramref name="nameFilter"/>.
        /// The <paramref name="nameFilter"/> is name, prefix or prefixed name (like ui:Binding).
        /// </summary>
        /// <param name="nameFilter">The current name or prefix to filter by.</param>
        /// <param name="iconHint">The recomended icon for completions.</param>
        /// <returns>The enumeration of completion items for curly components.</returns>
        IEnumerable<ICompletion> GetComponents(string nameFilter, ImageSource iconHint);

        /// <summary>
        /// Returns an enumeration of completions for attributes of <paramref name="currentSyntax"/>.
        /// </summary>
        /// <param name="currentSyntax">The curly syntax node to offer the attributes for.</param>
        /// <param name="nameFilter">The current attribute name to filter by.</param>
        /// <param name="iconHint">The recomended icon for completions.</param>
        /// <returns>The enumeration of completion items for the curly syntax node.</returns>
        IEnumerable<ICompletion> GetAttributes(CurlyNode currentSyntax, string nameFilter, ImageSource iconHint);
    }
}
