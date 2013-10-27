using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public abstract class TypeRegistryHelper
    {
        protected TypeBuilderRegistryConfiguration Configuration { get; private set; }
        protected TypeBuilderRegistryContent Content { get; private set; }

        public TypeRegistryHelper(TypeBuilderRegistryConfiguration configuration, TypeBuilderRegistryContent content)
        {
            Configuration = configuration;
            Content = content;
        }

        protected string PreparePrefix(string prefix)
        {
            if (prefix == null)
                prefix = String.Empty;
            else
                prefix = prefix.ToLowerInvariant();

            return prefix;
        }

        protected string PrepareName(string name, IEnumerable<string> suffix)
        {
            if (name == null)
                throw new ArgumentNullException("tagName");

            if (suffix == null)
                throw new ArgumentNullException("suffix");

            name = name.ToLowerInvariant();
            foreach (string suffixName in suffix)
            {
                if (name.EndsWith(suffixName))
                {
                    name = name.Substring(0, name.Length - suffixName.Length);
                    return name;
                }
            }

            return name;
        }
    }
}
