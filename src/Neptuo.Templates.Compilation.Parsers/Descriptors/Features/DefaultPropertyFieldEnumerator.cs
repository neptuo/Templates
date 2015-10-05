using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Descriptors.Features
{
    /// <summary>
    /// Implementation of <see cref="IDefaultFieldEnumerator"/> that uses <see cref="DefaultPropertyAttribute"/> to provide 'default' properties.
    /// </summary>
    public class DefaultPropertyFieldEnumerator : IDefaultFieldEnumerator
    {
        private readonly Type type;
        private readonly string[] defaultProperties;

        public DefaultPropertyFieldEnumerator(Type type)
        {
            Ensure.NotNull(type, "type");
            this.type = type;
            this.defaultProperties = type.GetCustomAttributes<DefaultPropertyAttribute>()
                .Select(a => a.Name)
                .ToArray();
        }

        public IEnumerator<IFieldDescriptor> GetEnumerator()
        {
            return type.GetProperties()
                .Where(p => p.CanWrite && defaultProperties.Contains(p.Name))
                .Select(p => new TypePropertyFieldDescriptor(p))
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
