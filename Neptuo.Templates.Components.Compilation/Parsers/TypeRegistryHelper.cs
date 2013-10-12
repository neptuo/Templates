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

        protected string PrepareName(string name, string suffix)
        {
            if (name == null)
                throw new ArgumentNullException("tagName");

            name = name.ToLowerInvariant();
            if (name.EndsWith(suffix))
                name = name.Substring(0, name.Length - suffix.Length);

            return name;
        }
    }
}
