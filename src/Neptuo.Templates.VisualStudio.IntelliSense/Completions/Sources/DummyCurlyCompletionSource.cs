using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions.Sources
{
    public class DummyCurlyCompletionSource : ICurlyCompletionSource
    {
        private readonly List<string> tokenNames = new List<string>() { "Binding", "TemplateBinding", "Template", "Source", "StaticResource" };

        public IEnumerable<string> GetNamespacesOrRootNames(string prefixOrNameFilter)
        {
            if (prefixOrNameFilter == null)
                return tokenNames;

            return tokenNames.Where(t => t.StartsWith(prefixOrNameFilter));
        }

        public IEnumerable<string> GetNamesInNamespace(string prefix, string nameFilter)
        {
            return tokenNames;
        }

        public ICurlyCompletionComponent FindComponent(string prefix, string name)
        {
            return new DummyCurlyCompletionComponent(prefix, name);
        }
    }

    public class DummyCurlyCompletionComponent : ICurlyCompletionComponent
    {
        private readonly List<string> attributeNames = new List<string>() { "Path", "Converter", "Key" };

        public string Prefix { get; private set; }
        public string Name { get; private set; }

        public DummyCurlyCompletionComponent(string prefix, string name)
        {
            Prefix = prefix;
            Name = name;
        }

        public IEnumerable<string> GetAttributeNames(string prefix, string attributeNameFilter)
        {
            if (attributeNameFilter == null)
                return attributeNames;

            return attributeNames.Where(a => a.StartsWith(attributeNameFilter));
        }
    }

}
