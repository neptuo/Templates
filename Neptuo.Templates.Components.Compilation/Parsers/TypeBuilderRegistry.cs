using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class TypeBuilderRegistry : IBuilderRegistry
    {
        public const string ObserverWildcard = "*";
        public const string ComponentSuffix = "control";
        public const string ObserverSuffix = "observer";

        protected NamespaceBuilderRegistryContent Content { get; private set; }

        public TypeBuilderRegistry(ILiteralBuilder literalBuilder, IComponentBuilder genericContentBuilder)
            : this(null)
        {
            Content.LiteralBuilder = literalBuilder;
            Content.GenericContentBuilder = genericContentBuilder;
        }

        internal TypeBuilderRegistry(NamespaceBuilderRegistryContent content = null)
        {
            Content = content ?? new NamespaceBuilderRegistryContent();
        }

        public IComponentBuilder GetComponentBuilder(string prefix, string name)
        {
            prefix = PreparePrefix(prefix);
            name = PrepareName(name, ComponentSuffix);

            if (Content.Components.ContainsKey(prefix)
                && Content.Components[prefix].ContainsKey(name))
            {
                IComponentBuilderFactory factory = Content.Components[prefix][name];
                return factory.CreateBuilder(prefix, name);
            }

            return GetGenericContentBuilder(name);
        }

        public IObserverRegistration GetObserverBuilder(string prefix, string name)
        {
            prefix = PreparePrefix(prefix);
            name = PrepareName(name, ComponentSuffix);

            if (!Content.Observers.ContainsKey(prefix))
                return null;

            IObserverBuilderFactory factory = null;

            if (Content.Observers[prefix].ContainsKey(name))
                factory = Content.Observers[prefix][name];
            else if (Content.Observers[prefix].ContainsKey(ObserverWildcard))
                factory = Content.Observers[prefix][ObserverWildcard];

            if (factory != null)
                return factory.CreateBuilder(prefix, name);

            return null;
        }

        public IComponentBuilder GetGenericContentBuilder(string name)
        {
            return Content.GenericContentBuilder;
        }

        public ILiteralBuilder GetLiteralBuilder()
        {
            return Content.LiteralBuilder;
        }

        #region Registration

        protected void RegisterNamespaceInternal(NamespaceDeclaration namespaceDeclaration)
        {
            string prefix = PreparePrefix(namespaceDeclaration.Prefix);
            if (!Content.Namespaces.ContainsKey(prefix))
                Content.Namespaces[prefix] = namespaceDeclaration;
        }

        protected string PreparePrefix(string prefix)
        {
            if (prefix == null)
                prefix = String.Empty;
            else
                prefix = prefix.ToLowerInvariant();

            return prefix;
        }

        protected string PrepareName(string name, string suffix)
        {
            if (name == null)
                throw new ArgumentNullException("tagName");

            name = name.ToLowerInvariant();
            if (name.EndsWith(suffix))
                name = name.Substring(0, name.Length - suffix.Length);

            return name;
        }

        protected void RegisterComponent(string prefix, string tagName, IComponentBuilderFactory factory)
        {
            prefix = PreparePrefix(prefix);
            tagName = PrepareName(tagName, ComponentSuffix);
            Content.Components[prefix][tagName] = factory;
        }

        protected void RegisterObserver(string prefix, string tagName, IObserverBuilderFactory factory)
        {
            prefix = PreparePrefix(prefix);
            tagName = PrepareName(tagName, ObserverSuffix);
            Content.Observers[prefix][tagName] = factory;
        }

        public void RegisterNamespace(NamespaceDeclaration namespaceDeclaration)
        {
            RegisterNamespaceInternal(namespaceDeclaration);
        }

        public void RegisterComponentBuilder(string prefix, string tagName, IComponentBuilderFactory factory)
        {
            RegisterNamespaceInternal(new NamespaceDeclaration(prefix, tagName));
            RegisterComponent(prefix, tagName, factory);
        }

        public void RegisterObserverBuilder(string prefix, string attributeName, IObserverBuilderFactory factory)
        {
            RegisterNamespaceInternal(new NamespaceDeclaration(prefix, attributeName));
            RegisterObserver(prefix, attributeName, factory);
        }

        #endregion

        public IEnumerable<NamespaceDeclaration> GetRegisteredNamespaces()
        {
            return Content.Namespaces.Values;
        }

        public IBuilderRegistry CreateChildRegistry()
        {
            return new TypeBuilderRegistry(new NamespaceBuilderRegistryContent(Content));
        }

        #region Contains

        public bool ContainsComponent(string prefix, string name)
        {
            prefix = PreparePrefix(prefix);
            name = PrepareName(name, ComponentSuffix);

            if(!Content.Components.ContainsKey(prefix))
                return false;

            return Content.Components[prefix].ContainsKey(name);
        }

        public bool ContainsObserver(string prefix, string name)
        {
            prefix = PreparePrefix(prefix);
            name = PrepareName(name, ObserverSuffix);

            if (!Content.Observers.ContainsKey(prefix))
                return false;

            if (Content.Components[prefix].ContainsKey(name))
                return true;

            return Content.Components[prefix].ContainsKey(ObserverWildcard);
        }

        #endregion
    }

}
