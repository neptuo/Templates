using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Extensible registry for sub parsers.
    /// </summary>
    public interface IParserRegistry
    {
        /// <summary>
        /// Returns <c>true</c> if <typeparamref name="T"/> is registered; otherwise returns <c>false</c>.
        /// </summary>
        /// <typeparam name="T">Type of parser to return.</typeparam>
        /// <returns><c>true</c> if <typeparamref name="T"/> is registered; otherwise returns <c>false</c>.</returns>
        bool Has<T>();

        /// <summary>
        /// Returns <c>true</c> if <typeparamref name="T"/> is registered also with name; otherwise returns <c>false</c>.
        /// </summary>
        /// <typeparam name="T">Type of parser to return.</typeparam>
        /// <param name="name">Name of required instance.</param>
        /// <returns><c>true</c> if <typeparamref name="T"/> is registered; otherwise returns <c>false</c>.</returns>
        bool Has<T>(string name);

        /// <summary>
        /// Returns instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of parser to return.</typeparam>
        /// <returns>Instance of <typeparamref name="T"/>.</returns>
        T With<T>();

        /// <summary>
        /// Returns named instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of parser to return.</typeparam>
        /// <param name="name">Name of required instance.</param>
        /// <returns>Instance of <typeparamref name="T"/>.</returns>
        T With<T>(string name);
    }
}
