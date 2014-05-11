using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Object that has type.
    /// </summary>
    public interface ITypeCodeObject
    {
        /// <summary>
        /// Object type.
        /// </summary>
        Type Type { get; set; }
    }
}
