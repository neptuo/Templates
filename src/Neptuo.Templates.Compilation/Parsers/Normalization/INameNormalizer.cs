using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Normalization
{
    /// <summary>
    /// Component for preprocessing names and prefixes.
    /// These names and prefixes can be used any parser.
    /// </summary>
    public interface INameNormalizer
    {
        /// <summary>
        /// Normalizes prefix.
        /// </summary>
        /// <param name="prefix">Prefix to normalize.</param>
        /// <returns>Normalized prefix.</returns>
        string PreparePrefix(string prefix);

        /// <summary>
        /// Normalizes name.
        /// </summary>
        /// <param name="name">Name to normalize.</param>
        /// <returns>Normalized name.</returns>
        string PrepareName(string name);
    }
}
