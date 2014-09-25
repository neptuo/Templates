using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Describes token structure, its properties and default property (if available).
    /// </summary>
    public interface ITokenDescriptor
    {
        /// <summary>
        /// Enumerates propertis.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IPropertyInfo> GetProperties();

        /// <summary>
        /// Gets default property. Can be <c>null</c>.
        /// </summary>
        /// <returns>Gets default property.</returns>
        IPropertyInfo GetDefaultProperty();
    }
}
