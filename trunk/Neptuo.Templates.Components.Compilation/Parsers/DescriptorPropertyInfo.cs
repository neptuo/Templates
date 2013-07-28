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
    }
}
