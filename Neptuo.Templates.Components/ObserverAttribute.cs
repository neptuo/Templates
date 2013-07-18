using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class ObserverAttribute : Attribute
    {
        public ObserverLivecycle Livecycle { get; set; }

        public ObserverAttribute()
            : this(ObserverLivecycle.PerControl)
        { }

        public ObserverAttribute(ObserverLivecycle livecycle)
        {
            Livecycle = livecycle;
        }
    }

    public enum ObserverLivecycle
    {
        PerAttribute, PerControl, PerPage
    }
}
