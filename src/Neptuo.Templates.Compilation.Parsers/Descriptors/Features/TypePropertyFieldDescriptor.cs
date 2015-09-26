using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Descriptors.Features
{
    public class TypePropertyFieldDescriptor : IFieldDescriptor
    {
        private readonly PropertyInfo propertyInfo;

        public string Name
        {
            get { return propertyInfo.Name; }
        }

        public Type FieldType
        {
            get { return propertyInfo.PropertyType; }
        }

        public TypePropertyFieldDescriptor(PropertyInfo propertyInfo)
        {
            Ensure.NotNull(propertyInfo, "propertyInfo");
            this.propertyInfo = propertyInfo;
        }
    }
}
