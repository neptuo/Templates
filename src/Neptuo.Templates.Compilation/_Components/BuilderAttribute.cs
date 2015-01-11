using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Used by <see cref="TypeScanner"/> for registering builder component.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class BuilderAttribute : Attribute
    {
        public Type[] Types { get; set; }

        public BuilderAttribute(params Type[] types)
        {
            Types = types;
        }
    }
}
