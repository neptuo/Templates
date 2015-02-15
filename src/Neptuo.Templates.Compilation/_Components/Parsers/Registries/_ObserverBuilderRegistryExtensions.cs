using Neptuo.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Common extensions for <see cref="ObserverBuilderRegistry"/>.
    /// </summary>
    public static class _ObserverBuilderRegistryExtensions
    {
        public static ObserverBuilderRegistry AddBuilder<TObserver>(this ObserverBuilderRegistry registry, string prefix, string name)
        {
            return registry.AddBuilder(prefix, name, new XmlDefaultTypeObserverBuilder(typeof(TObserver)));
        }

        public static ObserverBuilderRegistry AddHtmlAttributeBuilder<T>(this ObserverBuilderRegistry registry, Expression<Func<T, Dictionary<string, string>>> propertyGetter)
        {
            Guard.NotNull(registry, "registry");
            string propertyName = TypeHelper.PropertyName(propertyGetter);
            return registry.AddBuilder(
                null, 
                "*", 
                new XmlHtmlAttributeObserverBuilder(typeof(T), propertyName)
            );
        }
    }
}
