using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Descriptors.Features
{
    /// <summary>
    /// Contains type.
    /// </summary>
    public interface ITypeAware
    {
        /// <summary>
        /// The type.
        /// </summary>
        Type Type { get; }
    }
}
