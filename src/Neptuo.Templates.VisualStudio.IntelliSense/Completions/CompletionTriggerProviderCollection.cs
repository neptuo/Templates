using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    public class CompletionTriggerProviderCollection : ICompletionTriggerProvider
    {
        private readonly List<ICompletionTriggerProvider> providers = new List<ICompletionTriggerProvider>();

        /// <summary>
        /// Adds <paramref name="provider"/> at the end of collection.
        /// </summary>
        /// <param name="provider">Provider to add.</param>
        /// <returns>Self (for fluency).</returns>
        public CompletionTriggerProviderCollection Add(ICompletionTriggerProvider provider)
        {
            Ensure.NotNull(provider, "provider");
            providers.Add(provider);
            return this;
        }

        public IEnumerable<ITokenTrigger> GetStartTriggers()
        {
            return providers.SelectMany(p => p.GetStartTriggers());
        }

        public IEnumerable<ITokenTrigger> GetCommitTriggers()
        {
            return providers.SelectMany(p => p.GetCommitTriggers());
        }
    }
}
