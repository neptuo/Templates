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
        private string name;
        private PropertyDescriptor propertyDescriptor;

        public string Name
        {
            get
            {
                if (name == null)
                {
                    PropertyAttribute attribute = propertyDescriptor.Attributes.OfType<PropertyAttribute>().FirstOrDefault();
                    if (attribute != null && !String.IsNullOrEmpty(attribute.Name))
                        name = attribute.Name;
                    else
                        name = propertyDescriptor.Name;
                }
                return name;
            }
        }

        public Type Type
        {
            get { return propertyDescriptor.PropertyType; }
        }

        public bool IsReadOnly
        {
            get { return propertyDescriptor.IsReadOnly; }
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
