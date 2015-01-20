using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Object that has type.
    /// </summary>
    public interface ITypeCodeObject : ICodeObject
    {
        /// <summary>
        /// Object type.
        /// </summary>
        Type Type { get; set; }
    }
}
