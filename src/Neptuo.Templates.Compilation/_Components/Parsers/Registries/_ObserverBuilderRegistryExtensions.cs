using System;
using System.Collections.Generic;
using System.Linq;
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
            throw Guard.Exception.NotImplemented();
            return registry.AddBuilder(prefix, name, new DefaultTypeObserverBuilder(typeof(TObserver), null));
        }
    }
}
