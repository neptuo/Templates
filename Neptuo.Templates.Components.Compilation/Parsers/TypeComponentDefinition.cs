using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class TypeComponentDefinition : IComponentDefinition
    {
        private Type type;

        public TypeComponentDefinition(Type type)
        {
            this.type = type;
        }

        public IEnumerable<IPropertyInfo> GetProperties()
        {
            return type.GetProperties().Select(p => new TypePropertyInfo(p));
        }

        public IPropertyInfo GetDefaultProperty()
        {
            PropertyDescriptor defaultProperty = TypeDescriptor.GetDefaultProperty(type);
            if (defaultProperty != null)
                return new DescriptorPropertyInfo(defaultProperty);

            return null;
        }
    }
}
