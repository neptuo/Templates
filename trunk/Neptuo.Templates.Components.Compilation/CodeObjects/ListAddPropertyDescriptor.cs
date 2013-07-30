using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    public class ListAddPropertyDescriptor : IPropertyDescriptor, IDefaultPropertyValue
    {
        public IPropertyInfo Property { get; set; }
        public List<ICodeObject> Values { get; set; }
        public bool IsDefaultValue { get; set; }

        public ListAddPropertyDescriptor(IPropertyInfo propertyName)
        {
            Property = propertyName;
            Values = new List<ICodeObject>();
        }

        public ListAddPropertyDescriptor(IPropertyInfo propertyName, params ICodeObject[] values)
        {
            Property = propertyName;
            Values = new List<ICodeObject>(values);
        }

        public void SetValue(ICodeObject value)
        {
            Values.Add(value);
        }
    }
}
