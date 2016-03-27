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

        public IEnumerable<ICompletion> GetComponents(string nameFilter, ImageSource iconHint)
        {
            IEnumerable<Tuple<string, string>> source = registrations.SelectMany(r => r.Value.Select(t => new Tuple<string, string>(r.Key + ":" + t.Key, t.Key)));

            bool hasComma = false;
            if (!String.IsNullOrEmpty(nameFilter))
            {
                hasComma = nameFilter.Contains(":");
                source = source.Where(t => t.Item1.StartsWith(nameFilter));
            }

            return source.Select(t => new DefaultCompletion(t.Item1, hasComma ? t.Item2 : t.Item1, t.Item1, iconHint));
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
                        .Where(a => !usedAttributes.Contains(a.ToLowerInvariant()))
                        .Select(a => new DefaultCompletion(a, iconHint));
                }
            }

            return Enumerable.Empty<ICompletion>();
        }
    }
}
