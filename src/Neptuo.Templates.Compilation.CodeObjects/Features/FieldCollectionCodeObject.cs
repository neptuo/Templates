using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects.Features
{
    public class FieldCollectionCodeObject : IFieldCollectionCodeObject
    {
        private readonly Dictionary<string, ICodeProperty> properties = new Dictionary<string, ICodeProperty>();

        public IFieldCollectionCodeObject AddProperty(ICodeProperty property)
        {
            Ensure.NotNull(property, "property");
            properties[property.Name] = property;
            return this;
        }

        public bool TryGetProperty(string propertyName, out ICodeProperty property)
        {
            Ensure.NotNullOrEmpty(propertyName, "propertyName");
            return properties.TryGetValue(propertyName, out property);
        }

        public IEnumerable<ICodeProperty> EnumerateProperties()
        {
            return properties.Values;
        }
    }
}
