using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Generator for field names.
    /// </summary>
    public interface IFieldNameProvider
    {
        /// <summary>
        /// Gets next unique name.
        /// </summary>
        /// <returns>Gets next unique name.</returns>
        string GetName();
    }
}
