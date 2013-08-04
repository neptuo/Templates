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
            if (prefix == null)
                prefix = String.Empty;
            else
                prefix = prefix.ToLowerInvariant();

            name = name.ToLowerInvariant();

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
            return Content.GenericContentBuilder;
        }

        public ILiteralBuilder GetLiteralBuilder()
        {
            return Content.LiteralBuilder;
        }

        #region Registration

        public void RegisterNamespace(NamespaceDeclaration namespaceDeclaration)
        {
            Content.Namespaces.Add(namespaceDeclaration.Prefix, namespaceDeclaration);
            //TODO: Registr clr namespace
        }

        public void RegisterComponentBuilder(string prefix, string tagName, IComponentBuilderFactory factory)
        {
            //TODO: Validate type!
            prefix = prefix.ToLowerInvariant();
            tagName = tagName.ToLowerInvariant();
            Content.Components[prefix][tagName] = factory;
        }

        public void RegisterObserverBuilder(string prefix, string attributeName, IObserverBuilderFactory factory)
        {
            //TODO: Validate type!
            prefix = prefix.ToLowerInvariant();
            attributeName = attributeName.ToLowerInvariant();
            Content.Observers[prefix][attributeName] = factory;
        }

        #endregion

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
        public ILiteralBuilder LiteralBuilder { get; set; }
        public IComponentBuilder GenericContentBuilder { get; set; }

        public Dictionary<string, NamespaceDeclaration> Namespaces { get; protected set; }
        public SpecialDictionary<string, Dictionary<string, IComponentBuilderFactory>> Components { get; protected set; }
        public SpecialDictionary<string, Dictionary<string, IObserverBuilderFactory>> Observers { get; protected set; }

        public NamespaceBuilderRegistryContent()
            : this(null, null, null, null, null)
        { }

        public NamespaceBuilderRegistryContent(NamespaceBuilderRegistryContent content)
            : this(content.Namespaces, content.Components, content.Observers, content.LiteralBuilder, content.GenericContentBuilder)
        { }

        public NamespaceBuilderRegistryContent(
            Dictionary<string, NamespaceDeclaration> namespaces,
            SpecialDictionary<string, Dictionary<string, IComponentBuilderFactory>> controls,
            SpecialDictionary<string, Dictionary<string, IObserverBuilderFactory>> observers,
            ILiteralBuilder literalBuilder,
            IComponentBuilder genericContentBuilder)
        {
            if (namespaces != null)
                Namespaces = new Dictionary<string, NamespaceDeclaration>(namespaces);
            else
                Namespaces = new Dictionary<string, NamespaceDeclaration>();

            if (controls != null)
                Components = new SpecialDictionary<string, Dictionary<string, IComponentBuilderFactory>>(controls);
            else
                Components = new SpecialDictionary<string, Dictionary<string, IComponentBuilderFactory>>();

            if (observers != null)
                Observers = new SpecialDictionary<string, Dictionary<string, IObserverBuilderFactory>>(observers);
            else
                Observers = new SpecialDictionary<string, Dictionary<string, IObserverBuilderFactory>>();

            LiteralBuilder = literalBuilder;
            GenericContentBuilder = genericContentBuilder;
        }

        public class SpecialDictionary<TKey, TValue> : Dictionary<TKey, TValue>
            where TValue : new()
        {
            public new TValue this[TKey key]
            {
                get
                {
                    if (!base.ContainsKey(key))
                        base[key] = new TValue();

                    return base[key];
                }
                set { base[key] = value; }
            }

            public SpecialDictionary()
            { }

            public SpecialDictionary(IDictionary<TKey, TValue> dictionary)
                : base(dictionary)
            { }
        }
    }

}
