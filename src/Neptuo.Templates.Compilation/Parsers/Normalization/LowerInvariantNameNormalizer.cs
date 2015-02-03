using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Normalization
{
    /// <summary>
    /// Converts all names (name prefixes) to lower invariant version.
    /// <c>null</c> is converted to <see cref="String.Empty"/>.
    /// </summary>
    public class LowerInvariantNameNormalizer : INameNormalizer
    {
        public string PreparePrefix(string prefix)
        {
            if (prefix == null)
                return String.Empty;

            return prefix.ToLowerInvariant();
        }

        public string PrepareName(string name)
        {
            if (name == null)
                return String.Empty;

            return name.ToLowerInvariant();
        }
    }
}
