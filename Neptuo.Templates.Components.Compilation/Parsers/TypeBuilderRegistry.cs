using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class TypeBuilderRegistry : TypeRegistryHelper, IBuilderRegistry
    {
        #region Type scanner

        private object scannerLock = new object();
        private TypeScanner typeScanner;

        protected TypeScanner TypeScanner
        {
            get
            {
                if (typeScanner == null)
                {
                    lock (scannerLock)
                    {
                        if (typeScanner == null)
                            typeScanner = CreateTypeScanner();
                    }
                }

                return typeScanner;
            }
        }

        #endregion

        public TypeBuilderRegistry(TypeBuilderRegistryConfiguration configuration, ILiteralBuilder literalBuilder, IComponentBuilder genericContentBuilder)
            : this(configuration, null)
        {
            Content.LiteralBuilder = literalBuilder;
            Content.GenericContentBuilder = genericContentBuilder;
        }

        protected TypeBuilderRegistry(TypeBuilderRegistryConfiguration configuration, TypeBuilderRegistryContent content = null)
            : base(configuration, content ?? new TypeBuilderRegistryContent())
        { }

        #region Get

        public IComponentBuilder GetComponentBuilder(string prefix, string name)
        {
            prefix = PreparePrefix(prefix);
            name = PrepareName(name, Configuration.ComponentSuffix);

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
            name = PrepareName(name, Configuration.ComponentSuffix);

            if (!Content.Observers.ContainsKey(prefix))
                return null;

            IObserverBuilderFactory factory = null;

            if (Content.Observers[prefix].ContainsKey(name))
                factory = Content.Observers[prefix][name];
            else if (Content.Observers[prefix].ContainsKey(Configuration.ObserverWildcard))
                factory = Content.Observers[prefix][Configuration.ObserverWildcard];

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

        public IEnumerable<NamespaceDeclaration> GetRegisteredNamespaces()
        {
            return Content.Namespaces.Values;
        }

        #endregion

        #region Registration

        protected void RegisterNamespaceInternal(NamespaceDeclaration namespaceDeclaration)
        {
            string prefix = PreparePrefix(namespaceDeclaration.Prefix);
            if (!Content.Namespaces.ContainsKey(prefix))
                Content.Namespaces[prefix] = namespaceDeclaration;
        }

        protected void RegisterComponent(string prefix, string tagName, IComponentBuilderFactory factory)
        {
            prefix = PreparePrefix(prefix);
            tagName = PrepareName(tagName, Configuration.ComponentSuffix);
            Content.Components[prefix][tagName] = factory;
        }

        protected void RegisterObserver(string prefix, string tagName, IObserverBuilderFactory factory)
        {
            prefix = PreparePrefix(prefix);
            tagName = PrepareName(tagName, Configuration.ObserverSuffix);
            Content.Observers[prefix][tagName] = factory;
        }

        public void RegisterNamespace(NamespaceDeclaration namespaceDeclaration)
        {
            RegisterNamespaceInternal(namespaceDeclaration);
            TypeScanner.Scan(namespaceDeclaration.Prefix, namespaceDeclaration.Namespace);
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

        public IBuilderRegistry CreateChildRegistry()
        {
            return new TypeBuilderRegistry(Configuration, new TypeBuilderRegistryContent(Content));
        }

        #region Contains

        public bool ContainsComponent(string prefix, string name)
        {
            prefix = PreparePrefix(prefix);
            name = PrepareName(name, Configuration.ComponentSuffix);

            if(!Content.Components.ContainsKey(prefix))
                return false;

            return Content.Components[prefix].ContainsKey(name);
        }

        public bool ContainsObserver(string prefix, string name)
        {
            prefix = PreparePrefix(prefix);
            name = PrepareName(name, Configuration.ObserverSuffix);

            if (!Content.Observers.ContainsKey(prefix))
                return false;

            if (Content.Components[prefix].ContainsKey(name))
                return true;

            return Content.Components[prefix].ContainsKey(Configuration.ObserverWildcard);
        }

        #endregion

        protected virtual TypeScanner CreateTypeScanner()
        {
            return new TypeScanner(Configuration, Content);
        }
    }

}
