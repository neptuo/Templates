using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Code object containing enumeration of code objects.
    /// </summary>
    public class CodeObjectCollection : ICodeObject, IEnumerable<ICodeObject>
    {
        private readonly List<ICodeObject> storage = new List<ICodeObject>();

        public CodeObjectCollection(params ICodeObject[] codeObjects)
        {
            Ensure.NotNull(codeObjects, "codeObjects");

            foreach (ICodeObject codeObject in codeObjects)
                Add(codeObject);
        }

        /// <summary>
        /// Adds <paramref name="codeObject"/> to the collection.
        /// </summary>
        /// <param name="codeObject">Code object to add.</param>
        /// <returns>Self (for fluency).</returns>
        public CodeObjectCollection Add(ICodeObject codeObject)
        {
            if (codeObject != null)
                storage.Add(codeObject);

            return this;
        }

        public IEnumerator<ICodeObject> GetEnumerator()
        {
            return storage.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
