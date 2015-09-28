using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects.Features
{
    /// <summary>
    /// Default implementation of <see cref="INameCodeObject"/>.
    /// </summary>
    public class NameCodeObject : INameCodeObject
    {
        public string Name { get; private set; }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="name">Name of the code object.</param>
        public NameCodeObject(string name)
        {
            Name = name;
        }
    }
}
