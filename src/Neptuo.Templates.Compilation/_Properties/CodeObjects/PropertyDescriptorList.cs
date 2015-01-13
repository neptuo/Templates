using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Implementation of list of property descriptors.
    /// With support for extension methods.
    /// </summary>
    public class PropertyDescriptorList : Collection<IPropertyDescriptor>, IEnumerable<IPropertyDescriptor>
    {
        public PropertyDescriptorList(params IPropertyDescriptor[] values)
        {
            AddRange(values);
        }

        public new PropertyDescriptorList Add(IPropertyDescriptor value)
        {
            base.Add(value);
            return this;
        }

        public PropertyDescriptorList AddRange(IEnumerable<IPropertyDescriptor> values)
        {
            Guard.NotNull(values, "values");

            foreach (IPropertyDescriptor value in values)
                Add(value);

            return this;
        }

        protected override void InsertItem(int index, IPropertyDescriptor item)
        {
            Guard.NotNull(item, "item");
            base.InsertItem(index, item);
        }
    }
}
