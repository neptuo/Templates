using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Object that has properties.
    /// </summary>
    public interface IPropertiesCodeObject : ICodeObject
    {
        /// <summary>
        /// List of assigned properties.
        /// </summary>
        List<ICodeProperty> Properties { get; set; }
    }
}
