using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Defines code object with static/const value.
    /// </summary>
    public interface ILiteralCodeObject : ICodeObject
    {
        /// <summary>
        /// Literal value.
        /// </summary>
        object Value { get; }
    }
}
