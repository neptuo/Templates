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
            Ensure.NotNull(configuration, "configuration");
            return configuration.Get("IsDirectObjectResolve", true);
        }

        public static CodeDomDefaultConfiguration IsDirectObjectResolve(this CodeDomDefaultConfiguration configuration, bool value)
        {
            Ensure.NotNull(configuration, "configuration");
            configuration.Add("IsDirectObjectResolve", value);
            return configuration;
        }

        #endregion

        #region IsAttributeDefaultEnabled

        public static bool IsAttributeDefaultEnabled(this ICodeDomConfiguration configuration)
        {
            Ensure.NotNull(configuration, "configuration");
            return configuration.Get("IsAttributeDefaultEnabled", true);
        }

        public static CodeDomDefaultConfiguration IsAttributeDefaultEnabled(this CodeDomDefaultConfiguration configuration, bool value)
        {
            Ensure.NotNull(configuration, "configuration");
            configuration.Add("IsAttributeDefaultEnabled", value);
            return configuration;
        }

        #endregion

        #region IsPropertyTypeDefaultEnabled

        public static bool IsPropertyTypeDefaultEnabled(this ICodeDomConfiguration configuration)
        {
            Ensure.NotNull(configuration, "configuration");
            return configuration.Get("IsPropertyTypeDefaultEnabled", true);
        }

        public static CodeDomDefaultConfiguration IsPropertyTypeDefaultEnabled(this CodeDomDefaultConfiguration configuration, bool value)
        {
            Ensure.NotNull(configuration, "configuration");
            configuration.Add("IsPropertyTypeDefaultEnabled", value);
            return configuration;
        }

        #endregion
    }
}
