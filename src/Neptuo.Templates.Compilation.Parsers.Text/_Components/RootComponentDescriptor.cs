using Neptuo.Linq.Expressions;
using Neptuo.Models.Features;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Descriptors;
using Neptuo.Templates.Compilation.Parsers.Descriptors.Features;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class RootComponentDescriptor : DefaultComponentDescriptor, IFieldEnumerator, IDefaultFieldEnumerator
    {
        private readonly List<IFieldDescriptor> fields;

        public RootComponentDescriptor(PropertyInfo defaultProperty)
        {
            this.fields = new List<IFieldDescriptor>()
            {
                new TypePropertyFieldDescriptor(defaultProperty)
            };

            this
                .Add<IFieldEnumerator>(this)
                .Add<IDefaultFieldEnumerator>(this);
        }

        public IEnumerator<IFieldDescriptor> GetEnumerator()
        {
            return fields.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
