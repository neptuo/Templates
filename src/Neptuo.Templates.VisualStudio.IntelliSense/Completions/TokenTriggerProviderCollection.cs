using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    public class TokenTriggerProviderCollection : ITokenTriggerProvider
    {
        private readonly List<ITokenTriggerProvider> providers = new List<ITokenTriggerProvider>();

        /// <summary>
        /// Adds <paramref name="provider"/> at the end of collection.
        /// </summary>
        /// <param name="provider">Provider to add.</param>
        /// <returns>Self (for fluency).</returns>
        public TokenTriggerProviderCollection Add(ITokenTriggerProvider provider)
        {
            Ensure.NotNull(provider, "provider");
            providers.Add(provider);
            return this;
        }

        public IEnumerable<ITokenTrigger> GetTriggers()
        {
            return providers.SelectMany(p => p.GetTriggers());
        }
    }
}
