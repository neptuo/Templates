using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    public class NamespaceBuilderRegistry : IBuilderRegistry
    {
        public const string ObserverWildcard = "*";

        protected NamespaceBuilderRegistryContent Content { get; private set; }

        public NamespaceBuilderRegistry(NamespaceBuilderRegistryContent content = null)
        {
            Content = content ?? new NamespaceBuilderRegistryContent();
        }

        public IComponentBuilder GetComponentBuilder(string prefix, string name)
        {
            throw new NotImplementedException();
        }

        public IObserverRegistration GetObserverBuilder(string prefix, string name)
        {
            throw new NotImplementedException();
        }

        public void RegisterNamespace(NamespaceDeclaration namespaceDeclaration)
        {
            Content.Namespaces.Add(namespaceDeclaration.Prefix, namespaceDeclaration);
            //TODO: Registr clr namespace
        }

        public IEnumerable<NamespaceDeclaration> GetRegisteredNamespaces()
        {
            return Content.Namespaces.Values;
        }

        public IBuilderRegistry CreateChildRegistry()
        {
            return new NamespaceBuilderRegistry(new NamespaceBuilderRegistryContent(Content));
        }
    }

    public class NamespaceBuilderRegistryContent
    {
        public Dictionary<string, NamespaceDeclaration> Namespaces { get; protected set; }
        public Dictionary<string, Dictionary<string, Type>> ControlsInNamespaces { get; protected set; }
        public Dictionary<string, Dictionary<string, Type>> Observers { get; protected set; }

        public NamespaceBuilderRegistryContent()
            : this(null, null, null)
        { }

        public NamespaceBuilderRegistryContent(NamespaceBuilderRegistryContent content)
            : this(content.Namespaces, content.ControlsInNamespaces, content.Observers)
        { }

        public NamespaceBuilderRegistryContent(
            Dictionary<string, NamespaceDeclaration> namespaces, 
            Dictionary<string, Dictionary<string, Type>> controls, 
            Dictionary<string, Dictionary<string, Type>> observers)
        {
            Namespaces = new Dictionary<string, NamespaceDeclaration>(namespaces);
            ControlsInNamespaces = new Dictionary<string, Dictionary<string, Type>>(controls);
            Observers = new Dictionary<string, Dictionary<string, Type>>(observers);
        }
    }
}
