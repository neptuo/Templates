using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class BindPropertiesContext
    {
        public Dictionary<string, IPropertyInfo> Properties { get; private set; }
        public HashSet<string> BoundProperies { get; private set; }
        public List<XmlAttribute> UnboundAttributes { get; private set; }

        public BindPropertiesContext(Dictionary<string, IPropertyInfo> properties)
        {
            Properties = properties;
            BoundProperies = new HashSet<string>();
            UnboundAttributes = new List<XmlAttribute>();
        }
    }
}
