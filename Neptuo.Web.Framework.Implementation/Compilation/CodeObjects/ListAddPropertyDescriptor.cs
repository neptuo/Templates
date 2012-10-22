using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeObjects
{
    public class ListAddPropertyDescriptor : IPropertyDescriptor
    {
        public PropertyInfo Property { get; set; }
        public List<ICodeObject> Values { get; set; }

        public ListAddPropertyDescriptor(PropertyInfo property)
        {
            Property = property;
            Values = new List<ICodeObject>();
        }

        public void SetValue(ICodeObject value)
        {
            Values.Add(value);
        }
    }
}
