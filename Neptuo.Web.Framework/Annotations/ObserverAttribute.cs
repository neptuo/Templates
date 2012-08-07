using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Neptuo.Web.Framework.Utils;

namespace Neptuo.Web.Framework.Annotations
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class ObserverAttribute : Attribute
    {
        public ObserverLivecycle Livecycle { get; set; }

        public ObserverAttribute()
        {
            Livecycle = ObserverLivecycle.PerAttribute;
        }

        public static ObserverAttribute GetAttribute(Type prop)
        {
            return ReflectionHelper.GetAttribute<ObserverAttribute>(prop);
        }
    }

    public enum ObserverLivecycle
    {
        PerAttribute, PerControl, PerPage
    }
}
