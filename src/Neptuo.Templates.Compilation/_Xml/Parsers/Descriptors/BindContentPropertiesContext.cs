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
    public class BindContentPropertiesContext : BindPropertiesContext<IXmlAttribute>
    {
        /// <summary>
        /// Whether at least one property was bound from content element.
        /// </summary>
        public bool IsBoundFromContent { get; set; }

        public BindContentPropertiesContext(Dictionary<string, IPropertyInfo> properties)
            : base(properties)
        { }

        public BindContentPropertiesContext(IComponentDescriptor componentDescriptor)
            : base(componentDescriptor)
        { }

        public BindContentPropertiesContext(IComponentDescriptor componentDescriptor, IPropertiesCodeObject codeObject)
            : base(componentDescriptor, codeObject)
        { }
    }
}
