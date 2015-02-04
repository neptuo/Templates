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
        public static ContentBuilderRegistry AddGenericControlSearchHandler<T>(this ContentBuilderRegistry registry, Expression<Func<T, string>> tagNameProperty)
        {
            Guard.NotNull(registry, "registry");
            return registry.AddSearchHandler(e => new GenericContentControlBuilder<T>(tagNameProperty));
        }
    }
}
