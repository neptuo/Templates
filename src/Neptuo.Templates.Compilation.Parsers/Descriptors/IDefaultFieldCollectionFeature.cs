using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Descriptors
{
    /// <summary>
    /// Describes component, that has collection of default fields.
    /// </summary>
    public interface IDefaultFieldCollectionFeature
    {
        /// <summary>
        /// Enumerates default fields.
        /// </summary>
        /// <returns>Enumerates default fields.</returns>
        IEnumerable<IFieldDescriptor> DefaultFields { get; }
    }
}
