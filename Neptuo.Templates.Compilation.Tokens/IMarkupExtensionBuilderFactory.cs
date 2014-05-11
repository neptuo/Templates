using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Factor for <see cref="IMarkupExtensionBuilder"/>.
    /// </summary>
    public interface IMarkupExtensionBuilderFactory
    {
        /// <summary>
        /// Creates builder for <paramref name="prefix"/> and <paramref name="name"/>.
        /// </summary>
        /// <param name="prefix">Extension prefix.</param>
        /// <param name="name">Extension name.</param>
        /// <returns>Creates builder for <paramref name="prefix"/> and <paramref name="name"/>.</returns>
        IMarkupExtensionBuilder CreateBuilder(string prefix, string name);
    }
}
