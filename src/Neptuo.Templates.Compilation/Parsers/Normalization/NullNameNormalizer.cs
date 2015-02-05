using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Normalization
{
    /// <summary>
    /// Implementation of <see cref="INameNormalizer"/> which do nothing.
    /// </summary>
    public class NullNameNormalizer : INameNormalizer
    {
        public string PreparePrefix(string prefix)
        {
            return prefix;
        }

        public string PrepareName(string name)
        {
            return name;
        }
    }
}
