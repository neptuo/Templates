using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    public static class _IPropertyDescriptorExtensions
    {
        /// <summary>
        /// Sets all values from <paramref name="values"/> to the <paramref name="propertyDescriptor"/>.
        /// </summary>
        /// <param name="propertyDescriptor">Target property.</param>
        /// <param name="values">Enumeration of values to set to the <paramref name="propertyDescriptor"/>.</param>
        public static void SetRangeValue(this IPropertyDescriptor propertyDescriptor, IEnumerable<ICodeObject> values)
        {
            Guard.NotNull(propertyDescriptor, "propertyDescriptor");
            Guard.NotNull(values, "values");

            foreach (ICodeObject value in values)
                propertyDescriptor.SetValue(value);
        }

        /// <summary>
        /// Sets all values from <paramref name="values"/> to the <paramref name="propertyDescriptor"/>.
        /// </summary>
        /// <param name="propertyDescriptor">Target property.</param>
        /// <param name="values">Enumeration of values to set to the <paramref name="propertyDescriptor"/>.</param>
        public static void SetRangeValue(this IPropertyDescriptor propertyDescriptor, params ICodeObject[] values)
        {
            Guard.NotNull(propertyDescriptor, "propertyDescriptor");
            Guard.NotNull(values, "values");

            foreach (ICodeObject value in values)
                propertyDescriptor.SetValue(value);
        }
    }
}
