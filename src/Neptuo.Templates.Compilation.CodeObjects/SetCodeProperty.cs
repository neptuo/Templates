using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Property with single value.
    /// Every call to <see cref="ICodeProperty.SetValue"/> overrides previous value.
    /// </summary>
    public class SetCodeProperty : ICodeProperty
    {
        /// <summary>
        /// Current value.
        /// </summary>
        public ICodeObject Value { get; private set; }

        public string Name { get; private set; }
        public Type Type { get; set; }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="propertyType">Type of the property.</param>
        public SetCodeProperty(string name, Type propertyType)
        {
            Ensure.NotNull(name, "name");
            Ensure.NotNull(propertyType, "propertyType");
            Name = name;
            Type = propertyType;
        }

        public void SetValue(ICodeObject value)
        {
            Value = value;
        }
    }
}
