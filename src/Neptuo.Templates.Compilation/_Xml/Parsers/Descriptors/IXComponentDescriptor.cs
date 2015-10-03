using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Defines component.
    /// </summary>
    public interface IXComponentDescriptor
    {
        /// <summary>
        /// Enumerates properties.
        /// </summary>
        /// <returns>Enumerates properties.</returns>
        IEnumerable<IPropertyInfo> GetProperties();

        /// <summary>
        /// Gets default property.
        /// </summary>
        /// <returns>Gets default property.</returns>
        IPropertyInfo GetDefaultProperty();
    }
}
