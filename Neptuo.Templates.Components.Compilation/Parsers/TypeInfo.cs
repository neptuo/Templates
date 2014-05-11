using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Defines <see cref="IComponentInfo"/>, <see cref="IMarkupExtensionInfo"/> and <see cref="IObserverInfo"/> using class.
    /// </summary>
    public class TypeInfo : IComponentInfo, IMarkupExtensionInfo, IObserverInfo
    {
        private Type type;

        public TypeInfo(Type type)
        {
            Guard.NotNull(type, "type");
            this.type = type;
        }

        public IEnumerable<IPropertyInfo> GetProperties()
        {
            return type.GetProperties().Select(p => new TypePropertyInfo(p));
        }

        public IPropertyInfo GetDefaultProperty()
        {
            PropertyDescriptor defaultProperty = TypeDescriptor.GetDefaultProperty(type);
            if (defaultProperty != null)
                return new DescriptorPropertyInfo(defaultProperty);

            return null;
        }
    }
}
