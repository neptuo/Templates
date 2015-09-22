using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Default implementation of <see cref="IParserProvider"/>.
    /// </summary>
    public class DefaultParserRegistry : IParserProvider
    {
        private readonly Dictionary<Type, Dictionary<string, object>> storage = new Dictionary<Type, Dictionary<string, object>>();

        /// <summary>
        /// Registers instance <paramref name="parser"/> with interface <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of interface to register.</typeparam>
        /// <param name="parser">Instance to map to <typeparamref name="T"/>.</param>
        /// <returns>Self (fluently).</returns>
        public DefaultParserRegistry AddRegistry<T>(T parser)
        {
            return AddRegistry(String.Empty, parser);
        }

        /// <summary>
        /// Registers instance <paramref name="parser"/> with interface <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of interface to register.</typeparam>
        /// <param name="name">Name to register instance with.</param>
        /// <param name="parser">Instance to map to <typeparamref name="T"/>.</param>
        /// <returns>Self (fluently).</returns>
        public DefaultParserRegistry AddRegistry<T>(string name, T parser)
        {
            Ensure.NotNull(name, "name");
            Ensure.NotNull(parser, "parser");
            Type parserType = typeof(T);

            Dictionary<string, object> typeStorage;
            if (!storage.TryGetValue(parserType, out typeStorage))
                storage[parserType] = typeStorage = new Dictionary<string, object>();

            typeStorage[name] = parser;
            return this;
        }

        public bool Has<T>()
        {
            return Has<T>(String.Empty);
        }

        public bool Has<T>(string name)
        {
            Ensure.NotNull(name, "name");
            Type parserType = typeof(T);

            Dictionary<string, object> typeStorage;
            if (!storage.TryGetValue(parserType, out typeStorage))
                return false;

            return typeStorage.ContainsKey(name);
        }

        public T With<T>()
        {
            return With<T>(String.Empty);
        }

        public T With<T>(string name)
        {
            Ensure.NotNull(name, "name");
            Type parserType = typeof(T);

            Dictionary<string, object> typeStorage;
            if (storage.TryGetValue(parserType, out typeStorage))
            {
                object parser;
                if (typeStorage.TryGetValue(name, out parser))
                    return (T)parser;
            }

            throw Ensure.Exception.ArgumentOutOfRange("T", "Unnable to resolve parser of type '{0}' from parser registry.", parserType.FullName);
        }
    }
}
