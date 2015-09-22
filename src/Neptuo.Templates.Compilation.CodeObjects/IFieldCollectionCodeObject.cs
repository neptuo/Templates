using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Defines code object with collection of properties.
    /// </summary>
    public interface IFieldCollectionCodeObject
    {
        /// <summary>
        /// Adds <paramref name="property"/> to collection of properties.
        /// If collection already contains such property, previous value is overriden.
        /// </summary>
        /// <param name="property">Property to add.</param>
        void AddProperty(ICodeProperty property);

        /// <summary>
        /// Tries to get already set property by its name.
        /// </summary>
        /// <param name="propertyName">Name of the property to retrieve.</param>
        /// <param name="property">Retrieved property of <c>null</c>.</param>
        /// <returns><c>true</c> if collection contains such a property; <c>false</c> otherwise.</returns>
        bool TryGetProperty(string propertyName, out ICodeProperty property);

        /// <summary>
        /// Enumerates all set properties.
        /// </summary>
        /// <returns>Enumeration of already set properties.</returns>
        IEnumerable<ICodeProperty> EnumerateProperties();
    }
}
