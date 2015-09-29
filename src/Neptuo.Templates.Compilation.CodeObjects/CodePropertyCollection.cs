using System;
using System.Collections;
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
    public class CodePropertyCollection : IEnumerable<ICodeProperty>
    {
        private readonly List<ICodeProperty> storage = new List<ICodeProperty>();

        public CodePropertyCollection(params ICodeProperty[] codeProperties)
        {
            Ensure.NotNull(codeProperties, "codeProperties");

            foreach (ICodeProperty codeProperty in codeProperties)
                Add(codeProperty);
        }

        public CodePropertyCollection Add(ICodeProperty codeProperty)
        {
            Ensure.NotNull(codeProperty, "value");
            storage.Add(codeProperty);
            return this;
        }

        public IEnumerator<ICodeProperty> GetEnumerator()
        {
            return storage.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
