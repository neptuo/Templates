using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Observer.
    /// </summary>
    public class ObserverCodeObject : IObserverCodeObject
    {
        public Type Type { get; set; }
        public List<IPropertyDescriptor> Properties { get; set; }
        public bool IsNew { get; set; }

        public ObserverCodeObject(Type type)
        {
            Type = type;
            Properties = new List<IPropertyDescriptor>();
        }
    }
}
