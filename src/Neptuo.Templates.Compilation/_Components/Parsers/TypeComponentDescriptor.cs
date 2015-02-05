using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Implementation of <see cref="IComponentDescriptor"/> which uses reflection on type to read required properties.
    /// </summary>
    public class TypeComponentDescriptor : IComponentDescriptor
    {
        private readonly Type type;

        public TypeComponentDescriptor(Type type)
        {
            Guard.NotNull(type, "type");
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
                return new PropertyDescriptorPropertyInfo(defaultProperty);

            return null;
        }
    }
}
