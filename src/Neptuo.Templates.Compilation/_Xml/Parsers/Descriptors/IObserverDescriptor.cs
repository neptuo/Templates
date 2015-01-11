using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Describes observer, its properties.
    /// </summary>
    public interface IObserverDescriptor
    {
        /// <summary>
        /// Enumeration of properties of this observer.
        /// </summary>
        /// <returns>Enumeration of properties of this observer.</returns>
        IEnumerable<IPropertyInfo> GetProperties();
    }
}
