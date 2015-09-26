using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    public class ListAddCodeProperty : ICodeProperty
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

        public List<ICodeObject> Values { get; set; }
        public bool IsDefaultValue { get; set; }

        public ListAddCodeProperty(IPropertyInfo property)
        {
            this.property = property;
            Values = new List<ICodeObject>();
        }

        public ListAddCodeProperty(IPropertyInfo property, params ICodeObject[] values)
        {
            this.property = property;
            Values = new List<ICodeObject>(values);
        }

        public void SetValue(ICodeObject value)
        {
            Values.Add(value);
        }
    }
}
