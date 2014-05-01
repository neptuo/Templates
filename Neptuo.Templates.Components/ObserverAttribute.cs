using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates
{
    /// <summary>
    /// Provides metadata about observer.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class ObserverAttribute : Attribute
    {
        /// <summary>
        /// Observer live cycle scope.
        /// </summary>
        public ObserverLivecycle Livecycle { get; set; }

        public ObserverAttribute()
            : this(ObserverLivecycle.PerControl)
        { }

        public ObserverAttribute(ObserverLivecycle livecycle)
        {
            Livecycle = livecycle;
        }
    }
}
