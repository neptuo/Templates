using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    public class DictionaryAddPropertyDescriptor : IPropertyDescriptor
    {
        public IPropertyInfo Property { get; set; }
        public Dictionary<ICodeObject, ICodeObject> Values { get; set; }

        public ICodeObject CurrentKey { get; set; }

        public DictionaryAddPropertyDescriptor(IPropertyInfo property)
        {
            Guard.NotNull(property, "property");
            Property = property;
            Values = new Dictionary<ICodeObject, ICodeObject>();
        }

        public void SetValue(ICodeObject key, ICodeObject value)
        {
            Guard.NotNull(key, "key");
            Guard.NotNull(value, "value");
            Values[key] = value;
        }

        public void SetValue(ICodeObject value)
        {
            if (CurrentKey == null)
                CurrentKey = value;
            else
                SetValue(CurrentKey, value);
        }
    }
}
