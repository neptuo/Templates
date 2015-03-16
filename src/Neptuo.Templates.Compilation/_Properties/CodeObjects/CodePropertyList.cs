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
    public class CodePropertyList : Collection<ICodeProperty>, IEnumerable<ICodeProperty>
    {
        public CodePropertyList(params ICodeProperty[] values)
        {
            AddRange(values);
        }

        public new CodePropertyList Add(ICodeProperty value)
        {
            base.Add(value);
            return this;
        }

        public CodePropertyList AddRange(IEnumerable<ICodeProperty> values)
        {
            Ensure.NotNull(values, "values");

            foreach (ICodeProperty value in values)
                Add(value);

            return this;
        }

        protected override void InsertItem(int index, ICodeProperty item)
        {
            Ensure.NotNull(item, "item");
            base.InsertItem(index, item);
        }
    }
}
