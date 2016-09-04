using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Text
{
    /// <summary>
    /// Collection of composable tokenizers.
    /// </summary>
    public interface ITokenBuilderCollection
    {
        /// <summary>
        /// Adds <paramref name="tokenBuilder"/> to the collection.
        /// </summary>
        /// <param name="tokenBuilder">Token builder to add to the collection.</param>
        /// <returns>Self (for fluency).</returns>
        ITokenBuilderCollection Add(ITokenBuilder tokenBuilder);

        /// <summary>
        /// Returns enumeration of registered token builders.
        /// </summary>
        /// <returns>Enumeration of registered token builders.</returns>
        IEnumerable<ITokenBuilder> EnumerateBuilders();
    }
}
