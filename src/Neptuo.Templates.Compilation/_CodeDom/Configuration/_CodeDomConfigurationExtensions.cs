using Neptuo.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public static class _CodeDomConfigurationExtensions
    {
        #region IsDirectObjectResolve

        public static bool IsDirectObjectResolve(this ICodeDomConfiguration configuration)
        {
            Guard.NotNull(configuration, "configuration");
            return configuration.Get("IsDirectObjectResolve", (bool?)true);
        }

        public static CodeDomDefaultConfiguration IsDirectObjectResolve(this CodeDomDefaultConfiguration configuration, bool value)
        {
            Guard.NotNull(configuration, "configuration");
            configuration.Set("IsDirectObjectResolve", value);
            return configuration;
        }

        #endregion

        #region IsAttributeDefaultEnabled

        public static bool IsAttributeDefaultEnabled(this ICodeDomConfiguration configuration)
        {
            Guard.NotNull(configuration, "configuration");
            return configuration.Get("IsAttributeDefaultEnabled", (bool?)true);
        }

        public static CodeDomDefaultConfiguration IsAttributeDefaultEnabled(this CodeDomDefaultConfiguration configuration, bool value)
        {
            Guard.NotNull(configuration, "configuration");
            configuration.Set("IsAttributeDefaultEnabled", value);
            return configuration;
        }

        #endregion

        #region IsPropertyTypeDefaultEnabled

        public static bool IsPropertyTypeDefaultEnabled(this ICodeDomConfiguration configuration)
        {
            Guard.NotNull(configuration, "configuration");
            return configuration.Get("IsPropertyTypeDefaultEnabled", (bool?)true);
        }

        public static CodeDomDefaultConfiguration IsPropertyTypeDefaultEnabled(this CodeDomDefaultConfiguration configuration, bool value)
        {
            Guard.NotNull(configuration, "configuration");
            configuration.Set("IsPropertyTypeDefaultEnabled", value);
            return configuration;
        }

        #endregion
    }
}
