using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions.Sources
{
    public class DummyCurlyCompletionSource : ICurlyCompletionSource
    {
        private readonly Dictionary<string, Dictionary<string, List<string>>> registrations = new Dictionary<string, Dictionary<string, List<string>>>()
        {
            { 
                "data", 
                new Dictionary<string, List<string>>() 
                {
                    { "Binding", new List<string>() { "Path", "Converter" } },
                    { "TemplateBinding", new List<string>() { "Path", "Converter" } },
                    { "StaticResource", new List<string>() { "Key" } }
                }
            },
            {
                "ui",
                new Dictionary<string, List<string>>() 
                {
                    { "Template", new List<string>() { "Path" } }
                }
            }
        };

        public IEnumerable<ICompletion> GetComponents(string prefixOrNameFilter, ImageSource iconHint)
        {
            IEnumerable<string> source = registrations.SelectMany(r => r.Value.Select(t => r.Key + ":" + t.Key));

            if (!String.IsNullOrEmpty(prefixOrNameFilter))
                source = source.Where(t => t.StartsWith(prefixOrNameFilter));

            return source.Select(t => new DefaultCompletion(t, iconHint));
        }

        public IEnumerable<ICompletion> GetComponents(string prefix, string nameFilter, ImageSource iconHint)
        {
            if (prefix == null)
                prefix = String.Empty;
            else
                prefix = prefix.ToLowerInvariant();

            if (nameFilter == null)
                nameFilter = String.Empty;
            else
                nameFilter = nameFilter.ToLowerInvariant();

            IEnumerable<string> source = registrations
                .Where(r => r.Key == prefix)
                .SelectMany(r => r.Value.Where(t => t.Key.StartsWith(nameFilter)).Select(t => r.Key + ":" + t.Key));

            return source.Select(t => new DefaultCompletion(t, iconHint));
        }

        public IEnumerable<ICompletion> GetAttributes(CurlySyntax currentSyntax, string nameFilter, ImageSource iconHint)
        {
            string prefix = String.Empty;
            if (currentSyntax.Name.PrefixToken != null)
                prefix = currentSyntax.Name.PrefixToken.Text;

            string name = String.Empty;
            if (currentSyntax.Name.NameToken != null)
                name = currentSyntax.Name.NameToken.Text;

            Dictionary<string, List<string>> components;
            if (registrations.TryGetValue(prefix, out components))
            {
                List<string> component;
                if (components.TryGetValue(name, out component))
                {
                    HashSet<string> usedAttributes = new HashSet<string>(currentSyntax.Attributes
                        .Select(a => a.NameToken.Text.ToLowerInvariant())
                    );

                    return component
                        .Where(a => !usedAttributes.Contains(a))
                        .Select(a => new DefaultCompletion(a, iconHint));
                }
            }

            return Enumerable.Empty<ICompletion>();
        }
    }
}
