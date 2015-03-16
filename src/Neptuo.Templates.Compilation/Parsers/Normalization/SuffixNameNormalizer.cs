using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Normalization
{
    /// <summary>
    /// Given enumeration of suffixes for names, when name ends with one of these suffixes, this suffix is cut of the name.
    /// </summary>
    public class SuffixNameNormalizer : INameNormalizer
    {
        private readonly IEnumerable<string> nameSuffixes;

        public SuffixNameNormalizer(params string[] nameSuffixes)
        {
            Ensure.NotNull(nameSuffixes, "nameSuffixes");
            this.nameSuffixes = nameSuffixes;
        }

        public string PreparePrefix(string prefix)
        {
            if (prefix == null)
                prefix = String.Empty;

            return prefix;
        }

        public string PrepareName(string name)
        {
            if (name == null)
                return String.Empty;

            foreach (string nameSuffix in nameSuffixes)
            {
                if (name.EndsWith(nameSuffix))
                    return name.Substring(0, name.Length - nameSuffix.Length);
            }

            return name;
        }
    }
}
