using Neptuo.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public static class _ContentBuilderRegistryExtensions
    {
        public static ContentBuilderRegistry AddRootBuilder(this ContentBuilderRegistry registry, IContentBuilder builder)
        {
            Ensure.NotNull(registry, "registry");
            return registry.AddBuilder(null, TextXmlContentParser.FakeRootElementName, builder);
        }

        public static ContentBuilderRegistry AddRootBuilder<T>(this ContentBuilderRegistry registry, Expression<Func<T, ICollection<object>>> rootPropertyGetter)
        {
            return AddRootBuilder(
                registry,
                new RootContentBuilder(
                    new TypePropertyInfo(typeof(T).GetProperty(TypeHelper.PropertyName(rootPropertyGetter)))
                )
            );
        }

        public static ContentBuilderRegistry AddGenericControlSearchHandler<T>(this ContentBuilderRegistry registry, Expression<Func<T, string>> tagNameProperty)
        {
            Ensure.NotNull(registry, "registry");
            return registry.AddSearchHandler(e => new GenericContentControlBuilder<T>(tagNameProperty));
        }
    }
}
