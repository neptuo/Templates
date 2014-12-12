﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Smart 'dictionary' for registrations.
    /// </summary>
    public class TypeBuilderRegistryContent
    {
        public ILiteralBuilderFactory LiteralBuilderFactory { get; set; }
        public IContentBuilderFactory GenericContentBuilderFactory { get; set; }

        public Dictionary<string, NamespaceDeclaration> Namespaces { get; protected set; }
        public SpecialDictionary<string, Dictionary<string, IContentBuilder>> Components { get; protected set; }
        public SpecialDictionary<string, Dictionary<string, IObserverBuilderFactory>> Observers { get; protected set; }
        public SpecialDictionary<string, Dictionary<string, ITokenBuilderFactory>> Tokens { get; protected set; }
        public Dictionary<Type, IPropertyBuilder> Properties { get; protected set; }

        public TypeBuilderRegistryContent()
            : this(null, null, null, null, null, null, null)
        { }

        public TypeBuilderRegistryContent(TypeBuilderRegistryContent content)
            : this(content.Namespaces, content.Components, content.Observers, content.Tokens, content.Properties, content.LiteralBuilderFactory, content.GenericContentBuilderFactory)
        { }

        public TypeBuilderRegistryContent(
            Dictionary<string, NamespaceDeclaration> namespaces,
            SpecialDictionary<string, Dictionary<string, IContentBuilder>> controls,
            SpecialDictionary<string, Dictionary<string, IObserverBuilderFactory>> observers,
            SpecialDictionary<string, Dictionary<string, ITokenBuilderFactory>> tokens,
            Dictionary<Type, IPropertyBuilder> properties,
            ILiteralBuilderFactory literalBuilderFactory,
            IContentBuilderFactory genericContentBuilderFactory)
        {
            if (namespaces != null)
                Namespaces = new Dictionary<string, NamespaceDeclaration>(namespaces);
            else
                Namespaces = new Dictionary<string, NamespaceDeclaration>();

            if (controls != null)
                Components = new SpecialDictionary<string, Dictionary<string, IContentBuilder>>(controls);
            else
                Components = new SpecialDictionary<string, Dictionary<string, IContentBuilder>>();

            if (observers != null)
                Observers = new SpecialDictionary<string, Dictionary<string, IObserverBuilderFactory>>(observers);
            else
                Observers = new SpecialDictionary<string, Dictionary<string, IObserverBuilderFactory>>();

            if (tokens != null)
                Tokens = new SpecialDictionary<string, Dictionary<string, ITokenBuilderFactory>>(tokens);
            else
                Tokens = new SpecialDictionary<string, Dictionary<string, ITokenBuilderFactory>>();

            if (properties == null)
                Properties = new Dictionary<Type, IPropertyBuilder>();
            else
                Properties = properties;

            LiteralBuilderFactory = literalBuilderFactory;
            GenericContentBuilderFactory = genericContentBuilderFactory;
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

    public class NamespaceDeclaration
    {
        public string Prefix { get; set; }
        public string Namespace { get; set; }
    }
}
