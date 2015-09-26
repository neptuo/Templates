using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Descriptors.Features
{
    public class TypePropertyFieldEnumerator : IFieldEnumerator
    {
        private readonly Type type;

        public TypePropertyFieldEnumerator(Type type)
        {
            Ensure.NotNull(type, "type");
            this.type = type;
        }

        public IEnumerator<IFieldDescriptor> GetEnumerator()
        {
            return type.GetProperties()
                .Where(p => p.CanWrite)
                .Select(p => new TypePropertyFieldDescriptor(p))
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
