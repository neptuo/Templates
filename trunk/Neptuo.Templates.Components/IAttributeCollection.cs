using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates
{
    /// <summary>
    /// Enables setting attributes.
    /// </summary>
    public interface IAttributeCollection
    {
        /// <summary>
        /// Sest attribute <paramref name="name"/> with value <paramref name="value"/>.
        /// </summary>
        /// <param name="name">Attribute name.</param>
        /// <param name="value">Attribute value.</param>
        void SetAttribute(string name, string value);
    }
}
