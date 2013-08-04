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
        public PropertyInfo PropertyInfo { get; private set; }

        public string Name
        {
            get { return PropertyInfo.Name; }
        }

        public Type Type
        {
            get { return PropertyInfo.PropertyType; }
        }

        public bool IsReadOnly
        {
            get { return PropertyInfo.SetMethod == null || !PropertyInfo.SetMethod.IsPublic; }
        }

        public TypePropertyInfo(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
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
