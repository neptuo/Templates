using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Descriptors
{
    /// <summary>
    /// Defines component, that has collection of fields.
    /// </summary>
    public interface IFieldCollectionFeature
    {
        /// <summary>
        /// Enumerates fields.
        /// </summary>
        /// <returns>Enumerates fields.</returns>
        IEnumerable<IFieldDescriptor> Fields { get; }
    }
}
