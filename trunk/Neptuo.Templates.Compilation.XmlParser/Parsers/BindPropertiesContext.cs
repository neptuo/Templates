using Neptuo.Templates.Compilation.CodeObjects;
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
    public class BindPropertiesContext
    {
        /// <summary>
        /// List of all properties keyed by its name.
        /// </summary>
        public Dictionary<string, IPropertyInfo> Properties { get; private set; }

        /// <summary>
        /// List of properties, that already have values assigned.
        /// </summary>
        public HashSet<string> BoundProperies { get; private set; }

        /// <summary>
        /// List of not-processed attributes.
        /// </summary>
        public List<IXmlAttribute> UnboundAttributes { get; private set; }

        /// <summary>
        /// Creates new instance with <paramref name="properties"/> as list of all available properties.
        /// </summary>
        /// <param name="properties">List of all properties keyed by its name.</param>
        public BindPropertiesContext(Dictionary<string, IPropertyInfo> properties)
        {
            Guard.NotNull(properties, "properties");
            Properties = properties;
            BoundProperies = new HashSet<string>();
            UnboundAttributes = new List<IXmlAttribute>();
        }
    }
}
