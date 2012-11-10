using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeObjects
{
    public class DictionaryAddPropertyDescriptor : IPropertyDescriptor
    {
        protected string Key { get; set; }

        public Dictionary<string, ICodeObject> Values { get; protected set; }
        public PropertyInfo Property { get; set; }

        public void SetValue(ICodeObject value)
        {
            Values[Key] = value;
        }

        public void SetKey(string key)
        {
            Key = key;
        }
    }
}
