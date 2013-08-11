using Neptuo.Reflection;
using Neptuo.Templates.Compilation.CodeObjects;
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
        private string name;
        private PropertyInfo propertyInfo;

        public string Name
        {
            get
            {
                if (name == null)
                {
                    PropertyAttribute attribute = ReflectionHelper.GetAttribute<PropertyAttribute>(propertyInfo);
                    if (attribute != null && !String.IsNullOrEmpty(attribute.Name))
                        name = attribute.Name;
                    else
                        name = propertyInfo.Name;
                }
                return name;
            }
        }

        public Type Type
        {
            get { return propertyInfo.PropertyType; }
        }

        public bool IsReadOnly
        {
            get { return propertyInfo.SetMethod == null || !propertyInfo.SetMethod.IsPublic; }
        }

        public TypePropertyInfo(PropertyInfo propertyInfo)
        {
            this.propertyInfo = propertyInfo;
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
