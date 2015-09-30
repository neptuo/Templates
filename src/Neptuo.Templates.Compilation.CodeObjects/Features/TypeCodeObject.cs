using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects.Features
{
    /// <summary>
    /// Default implementation of <see cref="ITypeCodeObject"/>.
    /// </summary>
    public class TypeCodeObject : ITypeCodeObject
    {
        public Type Type { get; private set; }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="type">The type.</param>
        public TypeCodeObject(Type type)
        {
            Ensure.NotNull(type, "type");
            Type = type;
        }
    }
}
