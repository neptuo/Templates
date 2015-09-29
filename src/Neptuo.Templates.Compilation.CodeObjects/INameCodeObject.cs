using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Defines code object which takes unique name.
    /// </summary>
    public interface INameCodeObject : ICodeObject
    {
        /// <summary>
        /// Name of the code object.
        /// </summary>
        string Name { get; }
    }
}
