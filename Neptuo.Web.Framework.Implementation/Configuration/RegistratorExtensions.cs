using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Configuration
{
    public static class RegistratorExtensions
    {
        public static void LoadSection(this IRegistrator registrator, string name = "neptuo.web.framework/components")
        {
            ComponentsSection components = (ComponentsSection)ConfigurationManager.GetSection(name);
            foreach (ComponentElement element in components.Namespaces)
                registrator.RegisterNamespace(element.Prefix.EmptyToNull(), element.Namespace);

            foreach (ObserverElement element in components.Observers)
                registrator.RegisterObserver(element.Prefix.EmptyToNull(), element.Name.EmptyToNull(), element.Type);
        }

        internal static string EmptyToNull(this string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;

            return value;
        }
    }
}
