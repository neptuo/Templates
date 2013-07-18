using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Control.
    /// </summary>
    public class ControlCodeObject : IComponentCodeObject
    {
        public Type Type { get; set; }
        public List<IPropertyDescriptor> Properties { get; set; }
        public List<IObserverCodeObject> Observers { get; set; }

        public ControlCodeObject(Type type)
        {
            Type = type;
            Properties = new List<IPropertyDescriptor>();
            Observers = new List<IObserverCodeObject>();
        }
    }
}
