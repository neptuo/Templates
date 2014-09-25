using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Factor for <see cref="ITokenBuilder"/>.
    /// </summary>
    public interface ITokenBuilderFactory
    {
        /// <summary>
        /// Creates builder for <paramref name="prefix"/> and <paramref name="name"/>.
        /// </summary>
        /// <param name="prefix">Extension prefix.</param>
        /// <param name="name">Extension name.</param>
        /// <returns>Creates builder for <paramref name="prefix"/> and <paramref name="name"/>.</returns>
        ITokenBuilder CreateBuilder(string prefix, string name);
    }
}
