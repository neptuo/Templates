using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Descriptors
{
    public static class _ComponentDescriptorExtensions
    {
        public static bool TryWithDefaultFields(this IComponentDescriptor descriptor, out IDefaultFieldCollectionFeature defaultFields)
        {
            Ensure.NotNull(descriptor, "descriptor");
            return descriptor.TryWith<IDefaultFieldCollectionFeature>(out defaultFields);
        }

        public static bool TryWithFields(this IComponentDescriptor descriptor, out IFieldCollectionFeature fields)
        {
            Ensure.NotNull(descriptor, "descriptor");
            return descriptor.TryWith<IFieldCollectionFeature>(out fields);
        }
    }
}
