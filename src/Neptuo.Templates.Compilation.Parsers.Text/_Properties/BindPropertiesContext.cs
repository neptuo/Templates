using Neptuo.Models.Features;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Descriptors;
using Neptuo.Templates.Compilation.Parsers.Descriptors.Features;
using Neptuo.Templates.Compilation.Parsers.Normalization;
using System;
using System.Collections;
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
        public Dictionary<string, IFieldDescriptor> Fields { get; private set; }

        /// <summary>
        /// List of properties, that already have values assigned.
        /// </summary>
        public HashSet<string> BoundProperties { get; private set; }

        /// <summary>
        /// List of not-processed attributes.
        /// </summary>
        public List<T> UnboundAttributes { get; private set; }

        /// <summary>
        /// Creates new instance with <paramref name="fields"/> as list of all available properties.
        /// </summary>
        /// <param name="fields">List of all fields keyed by its name.</param>
        public BindPropertiesContext(Dictionary<string, IFieldDescriptor> fields)
        {
            Ensure.NotNull(fields, "fields");
            Fields = fields;
            BoundProperties = new HashSet<string>();
            UnboundAttributes = new List<T>();
        }

        public BindPropertiesContext(IComponentDescriptor componentDescriptor, INameNormalizer nameNormalizer)
            : this(componentDescriptor.With<IFieldEnumerator>().Where(IsBindableProperty).ToDictionary(p => nameNormalizer.PrepareName(p.Name)))
        { }

        public BindPropertiesContext(IComponentDescriptor componentDescriptor, INameNormalizer nameNormalizer, IFieldCollectionCodeObject codeObject)
            : this(componentDescriptor, nameNormalizer)
        {
            Ensure.NotNull(codeObject, "codeObject");
            foreach (ICodeProperty codeProperty in codeObject.EnumerateProperties())
                BoundProperties.Add(nameNormalizer.PrepareName(codeProperty.Name));
        }

        private static bool IsBindableProperty(IFieldDescriptor field)
        {
            if (!field.IsReadOnly)
                return true;

            if (typeof(string) == field.FieldType)
                return false;

            return typeof(IEnumerable).IsAssignableFrom(field.FieldType);
        }
    }
}