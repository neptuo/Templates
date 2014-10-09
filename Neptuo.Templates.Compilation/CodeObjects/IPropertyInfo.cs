using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Defines property.
    /// </summary>
    public interface IPropertyInfo
    {
        /// <summary>
        /// Property name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Property type.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Whether can be set or is read only.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Whether can be assigned type of <paramref name="type"/>.
        /// </summary>
        /// <param name="type">Type to assign.</param>
        /// <returns>Whether can be assigned type of <paramref name="type"/>.</returns>
        bool CanAssign(Type type);
    }
}
