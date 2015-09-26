using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    public class XSetCodeProperty : ICodeProperty
    {
        private readonly IPropertyInfo property;

        public string Name
        {
            get { return property.Name; }
        }

        public Type Type
        {
            get { return property.Type; }
        }

        public ICodeObject Value { get; set; }
        public bool IsDefaultValue { get; set; }

        public XSetCodeProperty(IPropertyInfo propertyName)
        {
            this.property = propertyName;
        }

        public XSetCodeProperty(IPropertyInfo propertyName, ICodeObject value)
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
