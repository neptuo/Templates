using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Factory for <see cref="IContentBuilder"/>.
    /// </summary>
    public interface IContentBuilderFactory
    {
        /// <summary>
        /// Creates <see cref="IContentBuilder"/> for <paramref name="prefix"/> and <paramref name="tagName"/>.
        /// </summary>
        /// <param name="prefix">Component prefix.</param>
        /// <param name="tagName">Component tag name.</param>
        /// <returns>Creates <see cref="IContentBuilder"/> for <paramref name="prefix"/> and <paramref name="tagName"/>.</returns>
        IContentBuilder CreateBuilder(string prefix, string tagName);
    }
}
