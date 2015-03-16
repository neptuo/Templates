using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Implementation of list of code objects.
    /// With support for extension methods.
    /// </summary>
    public class CodeObjectList : Collection<ICodeObject>, IEnumerable<ICodeObject>
    {
        public void AddRange(IEnumerable<ICodeObject> values)
        {
            Ensure.NotNull(values, "values");

            foreach (ICodeObject value in values)
                Add(value);
        }

        protected override void InsertItem(int index, ICodeObject item)
        {
            Ensure.NotNull(item, "item");
            base.InsertItem(index, item);
        }
    }
}
