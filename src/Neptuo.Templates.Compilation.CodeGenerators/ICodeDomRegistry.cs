using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Extensible registry for generators.
    /// </summary>
    public interface ICodeDomRegistry
    {
        /// <summary>
        /// Returns instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of generator to return.</typeparam>
        /// <returns>Instance of <typeparamref name="T"/>.</returns>
        T With<T>();
    }
}
