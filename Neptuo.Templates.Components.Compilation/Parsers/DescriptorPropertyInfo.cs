using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class DescriptorPropertyInfo : IPropertyInfo
    {
        private PropertyDescriptor propertyDescriptor;

        public string Name
        {
            get { return propertyDescriptor.Name; }
        }

        public Type Type
        {
            get { return propertyDescriptor.PropertyType; }
        }

        public DescriptorPropertyInfo(PropertyDescriptor propertyDescriptor)
        {
            this.propertyDescriptor = propertyDescriptor;
        }

        public bool CanAssign(Type type)
        {
            Type target = Type;
            if (Type.IsGenericType)
                target = Type.GetGenericArguments()[0];

            return target.IsAssignableFrom(type);
        }
    }
}
