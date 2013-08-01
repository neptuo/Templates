using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    public class TypeBuilderRegistry : IBuilderRegistry
    {
        public const string ObserverWildcard = "*";

        private ILiteralBuilder literalBuilder;
        private IComponentBuilder genericContentBuilder;

        protected NamespaceBuilderRegistryContent Content { get; private set; }

        public TypeBuilderRegistry(ILiteralBuilder literalBuilder, IComponentBuilder genericContentBuilder)
            : this(null)
        {
            this.literalBuilder = literalBuilder;
            this.genericContentBuilder = genericContentBuilder;
        }

        internal TypeBuilderRegistry(NamespaceBuilderRegistryContent content = null)
        {
            Content = content ?? new NamespaceBuilderRegistryContent();
        }

        public IComponentBuilder GetComponentBuilder(string prefix, string name)
        {
            if (prefix == null)
                prefix = String.Empty;
            else
                prefix = prefix.ToLowerInvariant();

            name = name.ToLowerInvariant();

            if (Content.ControlsInNamespaces.ContainsKey(prefix)
                && Content.ControlsInNamespaces[prefix].ContainsKey(name))
            {
                Type controlType = Content.ControlsInNamespaces[prefix][name];
                return new DefaultControlBuilder(controlType);
            }

            return GetGenericContentBuilder(name);
        }

        public IObserverRegistration GetObserverBuilder(string prefix, string name)
        {
            //if (attributeNamespace == null)
            //    attributeNamespace = String.Empty;
            //else
            //    attributeNamespace = attributeNamespace.ToLowerInvariant();

            //attributeName = attributeName.ToLowerInvariant();

            //if (!Observers.ContainsKey(attributeNamespace))
            //    return null;

            //if (Observers[attributeNamespace].ContainsKey(attributeName))
            //    return Observers[attributeNamespace][attributeName];

            //if (Observers[attributeNamespace].ContainsKey(ObserverWildcard))
            //    return Observers[attributeNamespace][ObserverWildcard];

            return null;
            //throw new NotImplementedException();
        }

        public IComponentBuilder GetGenericContentBuilder(string name)
        {
            return genericContentBuilder;
        }

        public ILiteralBuilder GetLiteralBuilder()
        {
            return literalBuilder;
        }

        public void RegisterNamespace(NamespaceDeclaration namespaceDeclaration)
        {
            Content.Namespaces.Add(namespaceDeclaration.Prefix, namespaceDeclaration);
            //TODO: Registr clr namespace
        }

        public IEnumerable<NamespaceDeclaration> GetRegisteredNamespaces()
        {
            return Content.Namespaces.Values;
            //TODO: Add observer ns!
        }

        public IBuilderRegistry CreateChildRegistry()
        {
            return new TypeBuilderRegistry(new NamespaceBuilderRegistryContent(Content));
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
            if (namespaces != null)
                Namespaces = new Dictionary<string, NamespaceDeclaration>(namespaces);
            else
                Namespaces = new Dictionary<string, NamespaceDeclaration>();

            if (controls != null)
                ControlsInNamespaces = new Dictionary<string, Dictionary<string, Type>>(controls);
            else
                ControlsInNamespaces = new Dictionary<string, Dictionary<string, Type>>();

            if (observers != null)
                Observers = new Dictionary<string, Dictionary<string, Type>>(observers);
            else
                Observers = new Dictionary<string, Dictionary<string, Type>>();
        }
    }
}
