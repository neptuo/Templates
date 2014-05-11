using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Static value object.
    /// </summary>
    public interface IPlainValueCodeObject : ICodeObject
    {
        /// <summary>
        /// Static value.
        /// </summary>
        object Value { get; set; }
    }
}
