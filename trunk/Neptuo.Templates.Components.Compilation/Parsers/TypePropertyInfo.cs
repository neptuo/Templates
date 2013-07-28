using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class TypePropertyInfo : IPropertyInfo
    {
        private PropertyInfo propertyInfo;

        public string Name
        {
            get { return propertyInfo.Name; }
        }

        public Type Type
        {
            get { return propertyInfo.PropertyType; }
        }

        public TypePropertyInfo(PropertyInfo propertyInfo)
        {
            this.propertyInfo = propertyInfo;
        }
    }
}
