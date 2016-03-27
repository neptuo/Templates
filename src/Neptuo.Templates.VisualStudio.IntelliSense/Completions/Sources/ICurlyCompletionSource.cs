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
        IEnumerable<ICompletion> GetComponents(string nameFilter, ImageSource iconHint);

        IEnumerable<ICompletion> GetAttributes(CurlySyntax currentSyntax, string nameFilter, ImageSource iconHint);
    }

    public interface ICurlyCompletionComponent
    {
        string Prefix { get; }
        string Name { get; }

        IEnumerable<string> GetAttributeNames(string prefix, string attributeNameFilter);
    }
}
