using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeObjects
{
    public class SetPropertyDescriptor : IPropertyDescriptor
    {
        public PropertyInfo Property { get; set; }
        public ICodeObject Value { get; set; }

        public SetPropertyDescriptor(PropertyInfo property)
        {
            Property = property;
        }

        public SetPropertyDescriptor(PropertyInfo property, ICodeObject value)
            : this(property)
        {
            SetValue(value);
        }

        public void SetValue(ICodeObject value)
        {
            Value = value;
        }
    }
}
