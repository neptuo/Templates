using System;
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
        public ILiteralBuilder LiteralBuilderFactory { get; set; }
        public IContentBuilder GenericContentBuilderFactory { get; set; }

        public Dictionary<string, NamespaceDeclaration> Namespaces { get; protected set; }
        public SpecialDictionary<string, Dictionary<string, IContentBuilder>> Components { get; protected set; }
        public SpecialDictionary<string, Dictionary<string, IObserverBuilder>> Observers { get; protected set; }
        public SpecialDictionary<string, Dictionary<string, ITokenBuilder>> Tokens { get; protected set; }
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
            SpecialDictionary<string, Dictionary<string, IObserverBuilder>> observers,
            SpecialDictionary<string, Dictionary<string, ITokenBuilder>> tokens,
            Dictionary<Type, IPropertyBuilder> properties,
            ILiteralBuilder literalBuilderFactory,
            IContentBuilder genericContentBuilderFactory)
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
                Observers = new SpecialDictionary<string, Dictionary<string, IObserverBuilder>>(observers);
            else
                Observers = new SpecialDictionary<string, Dictionary<string, IObserverBuilder>>();

            if (tokens != null)
                Tokens = new SpecialDictionary<string, Dictionary<string, ITokenBuilder>>(tokens);
            else
                Tokens = new SpecialDictionary<string, Dictionary<string, ITokenBuilder>>();

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
