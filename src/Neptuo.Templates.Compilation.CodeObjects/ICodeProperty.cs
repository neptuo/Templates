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
        /// Name of the property.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Type of the property.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Value setter.
        /// </summary>
        /// <param name="value">New value.</param>
        void SetValue(ICodeObject value);
    }
}
