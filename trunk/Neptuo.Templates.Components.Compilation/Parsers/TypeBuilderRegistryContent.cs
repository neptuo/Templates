using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class TypeBuilderRegistryContent
    {
        public ILiteralBuilder LiteralBuilder { get; set; }
        public IComponentBuilder GenericContentBuilder { get; set; }

        public Dictionary<string, NamespaceDeclaration> Namespaces { get; protected set; }
        public SpecialDictionary<string, Dictionary<string, IComponentBuilderFactory>> Components { get; protected set; }
        public SpecialDictionary<string, Dictionary<string, IObserverBuilderFactory>> Observers { get; protected set; }
        public SpecialDictionary<string, Dictionary<string, IMarkupExtensionBuilderFactory>> MarkupExtensions { get; protected set; }
        public Dictionary<Type, IPropertyBuilderFactory> Properties { get; protected set; }

        public TypeBuilderRegistryContent()
            : this(null, null, null, null, null, null, null)
        { }

        public TypeBuilderRegistryContent(TypeBuilderRegistryContent content)
            : this(content.Namespaces, content.Components, content.Observers, content.MarkupExtensions, content.Properties, content.LiteralBuilder, content.GenericContentBuilder)
        { }

        public TypeBuilderRegistryContent(
            Dictionary<string, NamespaceDeclaration> namespaces,
            SpecialDictionary<string, Dictionary<string, IComponentBuilderFactory>> controls,
            SpecialDictionary<string, Dictionary<string, IObserverBuilderFactory>> observers,
            SpecialDictionary<string, Dictionary<string, IMarkupExtensionBuilderFactory>> markupExtensions,
            Dictionary<Type, IPropertyBuilderFactory> properties,
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

            if (markupExtensions != null)
                MarkupExtensions = new SpecialDictionary<string, Dictionary<string, IMarkupExtensionBuilderFactory>>(markupExtensions);
            else
                MarkupExtensions = new SpecialDictionary<string, Dictionary<string, IMarkupExtensionBuilderFactory>>();

            if (properties == null)
                Properties = new Dictionary<Type, IPropertyBuilderFactory>();
            else
                Properties = properties;

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
