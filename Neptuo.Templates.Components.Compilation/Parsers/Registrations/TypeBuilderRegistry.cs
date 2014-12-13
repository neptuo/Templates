﻿using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Implementation of <see cref="IContentBuilderRegistry"/> and <see cref="ITokenBuilderFactory"/>.
    /// </summary>
    public class TypeBuilderRegistry : TypeRegistryHelper, IContentBuilder, ITokenBuilderFactory
    {
        private readonly TypeBuilderRegistry parentRegistry;

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

        public TypeBuilderRegistry(TypeBuilderRegistryConfiguration configuration, ILiteralBuilderFactory literalBuilderFactory, IContentBuilder genericContentBuilderFactory)
            : base(configuration, new TypeBuilderRegistryContent())
        {
            Guard.NotNull(literalBuilderFactory, "literalBuilderFactory");
            Guard.NotNull(genericContentBuilderFactory, "genericContentBuilder");
            Content.LiteralBuilderFactory = literalBuilderFactory;
            Content.GenericContentBuilderFactory = genericContentBuilderFactory;
        }

        protected TypeBuilderRegistry(TypeBuilderRegistry parentRegistry)
            : base(parentRegistry.Configuration, new TypeBuilderRegistryContent())
        {
            Guard.NotNull(parentRegistry, "parentRegistry");
            this.parentRegistry = parentRegistry;
        }

        #region Get component

        public ICodeObject TryParse(IContentBuilderContext context, IXmlElement element)
        {
            return GetComponentBuilder(element.Prefix, element.Name).TryParse(context, element);
        }

        public IContentBuilder GetComponentBuilder(string prefix, string name)
        {
            prefix = PreparePrefix(prefix);
            name = PrepareName(name, Configuration.ComponentSuffix);

            if (Content.Components[prefix].ContainsKey(name))
            {
                IContentBuilder factory = Content.Components[prefix][name];
                return factory;
            }

            if (parentRegistry != null && parentRegistry.ContainsComponent(prefix, name))
                return parentRegistry.GetComponentBuilder(prefix, name);

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

            if (parentRegistry != null)
                return parentRegistry.GetObserverBuilder(prefix, name);

            return null;
        }

        public IContentBuilder GetGenericContentBuilder(string name)
        {
            if (Content.GenericContentBuilderFactory == null)
            {
                if (parentRegistry != null)
                    return parentRegistry.GetGenericContentBuilder(name);

                throw new TypeBuilderRegistryException("Registry doesn't contain generic content builder.");
            }

            return Content.GenericContentBuilderFactory;
        }

        public ILiteralBuilder GetLiteralBuilder()
        {
            if (Content.LiteralBuilderFactory == null)
            {
                if (parentRegistry != null)
                    return parentRegistry.GetLiteralBuilder();

                throw new TypeBuilderRegistryException("Registry doesn't contain literal builder.");
            }

            return Content.LiteralBuilderFactory.CreateBuilder();
        }

        public IPropertyBuilder GetPropertyBuilder(IPropertyInfo propertyInfo)
        {
            if (!Content.Properties.ContainsKey(propertyInfo.Type))
            {
                if (parentRegistry != null)
                    return parentRegistry.GetPropertyBuilder(propertyInfo);

                return null;
            }

            return Content.Properties[propertyInfo.Type];
        }

        #endregion

        #region Get token

        public ITokenBuilder CreateBuilder(string prefix, string name)
        {
            prefix = PreparePrefix(prefix);
            name = PrepareName(name, Configuration.ExtensionSuffix);

            if (Content.Tokens[prefix].ContainsKey(name))
            {
                ITokenBuilderFactory factory = Content.Tokens[prefix][name];
                if (factory != null)
                    return factory.CreateBuilder(prefix, name);
            }

            if (parentRegistry != null)
                return parentRegistry.CreateBuilder(prefix, name);

            return null;
        }

        #endregion

        #region Registration

        protected void RegisterNamespaceInternal(string prefix, string clrNamespace)
        {
            prefix = PreparePrefix(prefix);
            if (!Content.Namespaces.ContainsKey(prefix))
                Content.Namespaces[prefix] = new NamespaceDeclaration() { Prefix = prefix, Namespace = clrNamespace };
        }

        protected void RegisterComponent(string prefix, string tagName, IContentBuilder factory)
        {
            prefix = PreparePrefix(prefix);
            tagName = PrepareName(tagName, Configuration.ComponentSuffix);
            Content.Components[prefix][tagName] = factory;
        }

        protected void RegisterToken(string prefix, string tagName, ITokenBuilderFactory factory)
        {
            prefix = PreparePrefix(prefix);
            tagName = PrepareName(tagName, Configuration.ExtensionSuffix);
            Content.Tokens[prefix][tagName] = factory;
        }

        protected void RegisterObserver(string prefix, string tagName, IObserverBuilderFactory factory)
        {
            prefix = PreparePrefix(prefix);
            tagName = PrepareName(tagName, Configuration.ObserverSuffix);
            Content.Observers[prefix][tagName] = factory;
        }

        public void RegisterNamespace(string prefix, string clrNamespace)
        {
            RegisterNamespaceInternal(prefix, clrNamespace);
            TypeScanner.Scan(prefix, clrNamespace);
        }

        public void RegisterComponentBuilder(string prefix, string tagName, IContentBuilder factory)
        {
            RegisterNamespaceInternal(prefix, tagName);
            RegisterComponent(prefix, tagName, factory);
        }

        public void RegisterExtensionBuilder(string prefix, string tagName, ITokenBuilderFactory factory)
        {
            RegisterNamespaceInternal(prefix, tagName);
            RegisterToken(prefix, tagName, factory);
        }

        public void RegisterObserverBuilder(string prefix, string attributeName, IObserverBuilderFactory factory)
        {
            RegisterNamespaceInternal(prefix, attributeName);
            RegisterObserver(prefix, attributeName, factory);
        }

        public void RegisterPropertyBuilder(Type propertyType, IPropertyBuilder factory)
        {
            Content.Properties[propertyType] = factory;
        }

        #endregion

        #region Contains

        public bool ContainsComponent(string prefix, string name)
        {
            prefix = PreparePrefix(prefix);
            name = PrepareName(name, Configuration.ComponentSuffix);

            if (!Content.Components.ContainsKey(prefix) || !Content.Components[prefix].ContainsKey(name))
            {
                if (parentRegistry != null)
                    return parentRegistry.ContainsComponent(prefix, name);

                return false;
            }

            return true;
        }

        public bool ContainsObserver(string prefix, string name)
        {
            prefix = PreparePrefix(prefix);
            name = PrepareName(name, Configuration.ObserverSuffix);

            if (!Content.Observers.ContainsKey(prefix) || (!Content.Observers[prefix].ContainsKey(name) && !Content.Observers[prefix].ContainsKey(Configuration.ObserverWildcard)))
            {
                if(parentRegistry != null)
                    return parentRegistry.ContainsObserver(prefix, name);

                return false;
            }

            return true;
        }

        public bool ContainsProperty(IPropertyInfo propertyInfo)
        {
            Guard.NotNull(propertyInfo, "propertyInfo");
            if (!Content.Properties.ContainsKey(propertyInfo.Type))
            {
                if (parentRegistry != null)
                    return parentRegistry.ContainsProperty(propertyInfo);

                return false;
            }

            return true;
        }

        #endregion

        protected virtual TypeScanner CreateTypeScanner()
        {
            return new TypeScanner(Configuration, Content);
        }
    }

}