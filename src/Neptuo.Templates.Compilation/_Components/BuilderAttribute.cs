using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Used by <see cref="AssemblyScanning.TypeScanner"/> for registering builder component.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class BuilderAttribute : Attribute
    {
        /// <summary>
        /// Enumeration of builder types.
        /// </summary>
        public Type[] Types { get; set; }

        /// <summary>
        /// Register builder types.
        /// </summary>
        /// <param name="types">Enumeration of builder types.</param>
        public BuilderAttribute(params Type[] types)
        {
            Types = types;
        }
    }
}
