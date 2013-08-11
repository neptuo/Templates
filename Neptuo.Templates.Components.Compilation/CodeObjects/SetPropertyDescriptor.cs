using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    public class SetPropertyDescriptor : IPropertyDescriptor
    {
        public IPropertyInfo Property { get; set; }
        public ICodeObject Value { get; set; }
        public bool IsDefaultValue { get; set; }

        public SetPropertyDescriptor(IPropertyInfo propertyName)
        {
            Property = propertyName;
        }

        public SetPropertyDescriptor(IPropertyInfo propertyName, ICodeObject value)
            : this(propertyName)
        {
            SetValue(value);
        }

        public void SetValue(ICodeObject value)
        {
            Value = value;
        }
    }
}
