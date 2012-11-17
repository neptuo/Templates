using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeObjects
{
    /// <summary>
    /// Observer.
    /// </summary>
    public class ObserverCodeObject : IObserverCodeObject
    {
        public Type Type { get; set; }
        public List<IPropertyDescriptor> Properties { get; set; }
        public ObserverLivecycle ObserverLivecycle { get; set; }

        public ObserverCodeObject(Type type, ObserverLivecycle observerLivecycle)
        {
            Type = type;
            ObserverLivecycle = observerLivecycle;
            Properties = new List<IPropertyDescriptor>();
        }
    }
}
