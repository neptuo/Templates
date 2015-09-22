using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Describes property.
    /// </summary>
    public interface ICodeProperty
    {
        /// <summary>
        /// Property info.
        /// </summary>
        IPropertyInfo Property { get; set; }

        /// <summary>
        /// Value setter.
        /// </summary>
        /// <param name="value">New value.</param>
        void SetValue(ICodeObject value);
    }
}
