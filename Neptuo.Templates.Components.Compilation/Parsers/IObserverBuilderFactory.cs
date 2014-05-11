using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Factory for <see cref="IObserverRegistration"/>.
    /// </summary>
    public interface IObserverBuilderFactory
    {
        /// <summary>
        /// Creates <see cref="IObserverRegistration"/> for <paramref name="prefix"/> and <paramref name="tagName"/>.
        /// </summary>
        /// <param name="prefix">Component prefix.</param>
        /// <param name="tagName">Component tag name.</param>
        /// <returns>Creates <see cref="IObserverRegistration"/> for <paramref name="prefix"/> and <paramref name="tagName"/>.</returns>
        IObserverRegistration CreateBuilder(string prefix, string attributeName);
    }
}
