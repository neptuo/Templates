using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Factory for <see cref="IComponentBuilder"/>.
    /// </summary>
    public interface IComponentBuilderFactory
    {
        /// <summary>
        /// Creates <see cref="IComponentBuilder"/> for <paramref name="prefix"/> and <paramref name="tagName"/>.
        /// </summary>
        /// <param name="prefix">Component prefix.</param>
        /// <param name="tagName">Component tag name.</param>
        /// <returns>Creates <see cref="IComponentBuilder"/> for <paramref name="prefix"/> and <paramref name="tagName"/>.</returns>
        IComponentBuilder CreateBuilder(string prefix, string tagName);
    }
}
