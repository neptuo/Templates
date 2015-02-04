using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Normalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Context of binding properties.
    /// </summary>
    public class BindPropertiesContext<T>
    {
        /// <summary>
        /// List of all properties keyed by its name.
        /// </summary>
        public Dictionary<string, IPropertyInfo> Properties { get; private set; }

        /// <summary>
        /// List of properties, that already have values assigned.
        /// </summary>
        public HashSet<string> BoundProperties { get; private set; }

        /// <summary>
        /// List of not-processed attributes.
        /// </summary>
        public List<T> UnboundAttributes { get; private set; }

        /// <summary>
        /// Creates new instance with <paramref name="properties"/> as list of all available properties.
        /// </summary>
        /// <param name="properties">List of all properties keyed by its name.</param>
        public BindPropertiesContext(Dictionary<string, IPropertyInfo> properties)
        {
            Guard.NotNull(properties, "properties");
            Properties = properties;
            BoundProperties = new HashSet<string>();
            UnboundAttributes = new List<T>();
        }

        public BindPropertiesContext(IComponentDescriptor componentDescriptor, INameNormalizer nameNormalizer)
            : this(componentDescriptor.GetProperties().Where(p => !p.IsReadOnly).ToDictionary(p => nameNormalizer.PrepareName(p.Name)))
        { }

        public BindPropertiesContext(IComponentDescriptor componentDescriptor, INameNormalizer nameNormalizer, IPropertiesCodeObject codeObject)
            : this(componentDescriptor, nameNormalizer)
        {
            Guard.NotNull(codeObject, "codeObject");
            foreach (ICodeProperty codeProperty in codeObject.Properties)
                BoundProperties.Add(nameNormalizer.PrepareName(codeProperty.Property.Name));
        }
    }
}
