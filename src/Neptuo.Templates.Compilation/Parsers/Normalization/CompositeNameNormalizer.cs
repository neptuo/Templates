using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Normalization
{
    /// <summary>
    /// Given enumeration of inner name normalizers, executes each one for each name (or prefix) with value from previous one (normalizer).
    /// </summary>
    public class CompositeNameNormalizer : INameNormalizer
    {
        private readonly IEnumerable<INameNormalizer> innerNormalizers;

        public CompositeNameNormalizer(params INameNormalizer[] innerNormalizers)
        {
            Ensure.NotNull(innerNormalizers, "innerNormalizers");
            this.innerNormalizers = innerNormalizers;
        }

        public string PreparePrefix(string prefix)
        {
            foreach (INameNormalizer innerNormalizer in innerNormalizers)
                prefix = innerNormalizer.PreparePrefix(prefix);

            return prefix;
        }

        public string PrepareName(string name)
        {
            foreach (INameNormalizer innerNormalizer in innerNormalizers)
                name = innerNormalizer.PrepareName(name);

            return name;
        }
    }
}
