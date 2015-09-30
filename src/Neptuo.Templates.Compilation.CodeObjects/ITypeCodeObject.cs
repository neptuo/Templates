using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Defines code object which takes component type.
    /// </summary>
    public interface ITypeCodeObject : ICodeObject
    {
        /// <summary>
        /// The type.
        /// </summary>
        Type Type { get; }
    }
}
