using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    public class DictionaryAddCodeProperty : ICodeProperty
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

        public Dictionary<ICodeObject, ICodeObject> Values { get; set; }

        public ICodeObject CurrentKey { get; set; }

        public DictionaryAddCodeProperty(IPropertyInfo property)
        {
            Ensure.NotNull(property, "property");
            this.property = property;
            Values = new Dictionary<ICodeObject, ICodeObject>();
        }

        public void SetValue(ICodeObject key, ICodeObject value)
        {
            Ensure.NotNull(key, "key");
            Ensure.NotNull(value, "value");
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
