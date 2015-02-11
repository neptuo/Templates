using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Provides ability to transform name of the pipeline for each processing part.
    /// Eg. View service is called with "Default", for parsers this is renamed to "MyParser", for code generators this is renamed to "ServerCode".
    /// These transitions are based on service type.
    /// </summary>
    public class DefaultPipelineDispatcher
    {
        private readonly Dictionary<Type, Dictionary<string, string>> storage = new Dictionary<Type, Dictionary<string, string>>();

        /// <summary>
        /// Registers dispatching rule for service of type <typeparamref name="TService"/> 
        /// for input name <paramref name="sourceName"/> to target name <paramref name="targetName"/>.
        /// </summary>
        /// <typeparam name="TService">Type of service for this rule.</typeparam>
        /// <param name="sourceName">Input pipeline name.</param>
        /// <param name="targetName">Target pipeline name for <typeparamref name="TService"/>.</param>
        /// <returns>Self (for fluency).</returns>
        public DefaultPipelineDispatcher Add<TService>(string sourceName, string targetName)
        {
            Guard.NotNull(sourceName, "sourceName");
            Guard.NotNull(targetName, "targetName");

            Type serviceType = typeof(TService);
            Dictionary<string, string> subStorage;
            if (!storage.TryGetValue(serviceType, out subStorage))
                storage[serviceType] = subStorage = new Dictionary<string, string>();

            subStorage[sourceName] = targetName;
            return this;
        }

        /// <summary>
        /// Returns target pipeline name for service of type <typeparamref name="TService"/> and input name <paramref name="sourceName"/>.
        /// </summary>
        /// <typeparam name="TService">Type of service.</typeparam>
        /// <param name="sourceName">Input pipeline name.</param>
        /// <returns>Target pipeline name.</returns>
        public string Dispatch<TService>(string sourceName)
        {
            Guard.NotNull(sourceName, "sourceName");

            Type serviceType = typeof(TService);
            Dictionary<string, string> subStorage;
            if (!storage.TryGetValue(serviceType, out subStorage))
                return sourceName;

            string targetName;
            if (!subStorage.TryGetValue(sourceName, out targetName))
                return sourceName;

            return targetName;
        }
    }
}
