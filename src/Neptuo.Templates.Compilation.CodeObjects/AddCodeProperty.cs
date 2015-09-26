using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Property with collection of items.
    /// Every call to <see cref="ICodeProperty.SetValue"/> adds item to collection of values.
    /// </summary>
    public class AddCodeProperty : ICodeProperty
    {
        /// <summary>
        /// Collection of all values.
        /// </summary>
        public ICollection<ICodeObject> Values { get; private set; }

        public string Name { get; private set; }
        public Type Type { get; private set; }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="propertyType">Type of the property.</param>
        public AddCodeProperty(string name, Type propertyType)
        {
            Ensure.NotNull(name, "name");
            Ensure.NotNull(propertyType, "propertyType");
            Name = name;
            Type = propertyType;
            Values = new List<ICodeObject>();
        }

        public void SetValue(ICodeObject value)
        {
            if (value != null)
                Values.Add(value);
        }
    }
}
