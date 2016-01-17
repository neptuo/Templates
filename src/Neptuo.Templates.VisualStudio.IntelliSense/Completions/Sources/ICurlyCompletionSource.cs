using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions.Sources
{
    /// <summary>
    /// The provider of registered namespaces of curly components.
    /// </summary>
    public interface ICurlyCompletionSource
    {
        IEnumerable<string> GetNamespacesOrRootNames(string prefixOrNameFilter);
        IEnumerable<string> GetNamesInNamespace(string prefix, string nameFilter);

        ICurlyCompletionComponent FindComponent(string prefix, string name);
    }

    public interface ICurlyCompletionComponent
    {
        string Prefix { get; }
        string Name { get; }

        IEnumerable<string> GetAttributeNames(string prefix, string attributeNameFilter);
    }
}
