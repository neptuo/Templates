using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    public class ListAddPropertyDescriptor : IPropertyDescriptor, IDefaultPropertyValue
    {
        public PropertyInfo Property { get; set; }
        public List<ICodeObject> Values { get; set; }
        public bool IsDefaultValue { get; set; }

        public ListAddPropertyDescriptor(PropertyInfo property)
        {
            Property = property;
            Values = new List<ICodeObject>();
        }

        public ListAddPropertyDescriptor(PropertyInfo property, params ICodeObject[] values)
        {
            Property = property;
            Values = new List<ICodeObject>(values);
        }

        public void SetValue(ICodeObject value)
        {
            Values.Add(value);
        }
    }
}
