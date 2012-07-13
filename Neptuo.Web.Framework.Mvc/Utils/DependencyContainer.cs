using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Html.Compilation;

namespace Neptuo.Web.Mvc.ViewEngine.Utils
{
    public class DependencyContainer : IDependencyContainer
    {
        Dictionary<Type, object> instances = new Dictionary<Type, object>();

        public object this[Type index]
        {
            get
            {
                object value = null;
                if (instances.TryGetValue(index, out value))
                    return value;

                return null;
            }
            set
            {
                if (instances.ContainsKey(index))
                    instances[index] = value;
                else
                    instances.Add(index, value);
            }
        }
    }
}
