using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// List of registration of <see cref="ITokenBuilder"/>.
    /// </summary>
    public interface ITokenBuilderRegistry
    {
        /// <summary>
        /// Gets registered <see cref="ITokenBuilder "/> for <paramref name="prefix"/> and <paramref name="name"/> or null.
        /// </summary>
        /// <param name="prefix">Extension prefix.</param>
        /// <param name="name">Exntesion name.</param>
        /// <returns>Gets registered <see cref="ITokenBuilder "/> for <paramref name="prefix"/> and <paramref name="name"/> or null.</returns>
        ITokenBuilder GetTokenBuilder(string prefix, string name);
    }
}
