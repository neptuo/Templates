using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Observer.
    /// </summary>
    public class ObserverCodeObject : ITypeCodeObject, IPropertiesCodeObject
    {
        public Type Type { get; set; }
        public List<ICodeProperty> Properties { get; set; }

        public ObserverCodeObject(Type type)
        {
            Guard.NotNull(type, "type");
            Type = type;
            Properties = new List<ICodeProperty>();
        }
    }
}
