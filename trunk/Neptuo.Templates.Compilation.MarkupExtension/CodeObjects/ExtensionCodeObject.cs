using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    public class ExtensionCodeObject : ITypeCodeObject, IPropertiesCodeObject
    {
        public Type Type { get; set; }
        public List<IPropertyDescriptor> Properties { get; set; }

        public ExtensionCodeObject(Type type)
        {
            Type = type;
            Properties = new List<IPropertyDescriptor>();
        }
    }
}
