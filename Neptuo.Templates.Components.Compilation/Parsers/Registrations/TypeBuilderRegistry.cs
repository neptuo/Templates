using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Tokens;
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
    public class TypeBuilderRegistry : TypeRegistryHelper, IContentBuilder, ILiteralBuilder, ITokenBuilder, IPropertyBuilder, IObserverBuilder
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

        public ILiteralBuilder LiteralBuilder
        {
            get { return Content.LiteralBuilderFactory; }
            set { Content.LiteralBuilderFactory = value; }
        }

        public IContentBuilder GenericContentBuilder
        {
            get { return Content.GenericContentBuilderFactory; }
            set { Content.GenericContentBuilderFactory = value; }
        }

        public TypeBuilderRegistry(TypeBuilderRegistryConfiguration configuration)
            : base(configuration, new TypeBuilderRegistryContent())
        { }

        public TypeBuilderRegistry(TypeBuilderRegistryConfiguration configuration, ILiteralBuilder literalBuilderFactory, IContentBuilder genericContentBuilderFactory)
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

        #region IContentBuilder

        ICodeObject IContentBuilder.TryParse(IContentBuilderContext context, IXmlElement element)
        {
            return GetComponentBuilder(element.Prefix, element.LocalName).TryParse(context, element);
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

        #endregion

        #region IObserverBuilder

        bool IObserverBuilder.TryParse(IContentBuilderContext context, IComponentCodeObject codeObject, IXmlAttribute attribute)
        {
            IObserverBuilder observerBuilder = GetObserverBuilder(attribute.Prefix, attribute.LocalName);
            if (observerBuilder == null)
                return false;

            return observerBuilder.TryParse(context, codeObject, attribute);
        }

        public IObserverBuilder GetObserverBuilder(string prefix, string name)
        {
            prefix = PreparePrefix(prefix);
            name = PrepareName(name, Configuration.ComponentSuffix);

            if (!Content.Observers.ContainsKey(prefix))
                return null;

            IObserverBuilder factory = null;

            if (Content.Observers[prefix].ContainsKey(name))
                factory = Content.Observers[prefix][name];
            else if (Content.Observers[prefix].ContainsKey(Configuration.ObserverWildcard))
                factory = Content.Observers[prefix][Configuration.ObserverWildcard];

            if (factory != null)
                return factory;

            if (parentRegistry != null)
                return parentRegistry.GetObserverBuilder(prefix, name);

            return null;
        }

        #endregion

        #region ILiteralBuilder

        ICodeObject ILiteralBuilder.TryParseText(IContentBuilderContext context, string text)
        {
            throw new NotImplementedException();
        }

        ICodeObject ILiteralBuilder.TryParseComment(IContentBuilderContext context, string commentText)
        {
            throw new NotImplementedException();
        }

        public ILiteralBuilder GetLiteralBuilder()
        {
            if (Content.LiteralBuilderFactory == null)
            {
                if (parentRegistry != null)
                    return parentRegistry.GetLiteralBuilder();

                throw new TypeBuilderRegistryException("Registry doesn't contain literal builder.");
            }

            return Content.LiteralBuilderFactory;
        }

        #endregion

        #region IPropertyBuilder

        bool IPropertyBuilder.TryParse(IContentBuilderContext context, IPropertiesCodeObject codeObject, IPropertyInfo propertyInfo, IEnumerable<IXmlNode> content)
        {
            IPropertyBuilder propertyBuilder = GetPropertyBuilder(propertyInfo);
            if (propertyBuilder == null)
                propertyBuilder = new TypeDefaultPropertyBuilder();

            return propertyBuilder.TryParse(context, codeObject, propertyInfo, content);
        }

        bool IPropertyBuilder.TryParse(IContentBuilderContext context, IPropertiesCodeObject codeObject, IPropertyInfo propertyInfo, ISourceContent attributeValue)
        {
            IPropertyBuilder propertyBuilder = GetPropertyBuilder(propertyInfo);
            if (propertyBuilder == null)
                return false;

            return propertyBuilder.TryParse(context, codeObject, propertyInfo, attributeValue);
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

        #region ITokenBuilder

        ICodeObject ITokenBuilder.TryParse(ITokenBuilderContext context, Token token)
        {
            ITokenBuilder tokenBuilder = GetTokenBuilder(token.Prefix, token.Name);
            if (tokenBuilder != null)
                return tokenBuilder.TryParse(context, token);

            return null;
        }

        public ITokenBuilder GetTokenBuilder(string prefix, string name)
        {
            prefix = PreparePrefix(prefix);
            name = PrepareName(name, Configuration.ExtensionSuffix);

            if (Content.Tokens[prefix].ContainsKey(name))
            {
                ITokenBuilder tokenBuilder = Content.Tokens[prefix][name];
                if (tokenBuilder != null)
                    return tokenBuilder;
            }

            if (parentRegistry != null)
                return parentRegistry.GetTokenBuilder(prefix, name);

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

        public void RegisterNamespace(string prefix, string clrNamespace)
        {
            RegisterNamespaceInternal(prefix, clrNamespace);
            TypeScanner.Scan(prefix, clrNamespace);
        }


        protected void RegisterComponent(string prefix, string tagName, IContentBuilder factory)
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

        public void RegisterComponentBuilder(string prefix, string tagName, IContentBuilder factory)
        {
            RegisterNamespaceInternal(prefix, tagName);
            RegisterComponent(prefix, tagName, factory);
        }


        protected void RegisterTokenInternal(string prefix, string tagName, ITokenBuilder factory)
        {
            prefix = PreparePrefix(prefix);
            tagName = PrepareName(tagName, Configuration.ExtensionSuffix);
            Content.Tokens[prefix][tagName] = factory;
        }

        public void RegisterTokenBuilder(string prefix, string tagName, ITokenBuilder factory)
        {
            RegisterNamespaceInternal(prefix, tagName);
            RegisterTokenInternal(prefix, tagName, factory);
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
            return new TypeScanner(Configuration, Content, this);
        }
    }

}
