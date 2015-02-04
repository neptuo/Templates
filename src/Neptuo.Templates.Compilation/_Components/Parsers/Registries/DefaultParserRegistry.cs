using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Default implementation of <see cref="IParserRegistry"/>.
    /// </summary>
    public class DefaultParserRegistry : IParserRegistry
    {
        private readonly Dictionary<Type, object> storage = new Dictionary<Type, object>();

        /// <summary>
        /// Registers instance <paramref name="parser"/> with interface <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of interface to register.</typeparam>
        /// <param name="parser">Instance to map to <typeparamref name="T"/>.</param>
        /// <returns>Self (fluently).</returns>
        public DefaultParserRegistry AddRegistry<T>(T parser)
        {
            Guard.NotNull(parser, "parser");
            storage[typeof(T)] = parser;
            return this;
        }

        public bool Has<T>()
        {
            Type parserType = typeof(T);
            return storage.ContainsKey(parserType);
        }

        public T With<T>()
        {
            Type parserType = typeof(T);
            object parser;
            if (!storage.TryGetValue(parserType, out parser))
                throw Guard.Exception.ArgumentOutOfRange("T", "Unnable to resolve parser of type '{0}' from parser registry.", parserType.FullName);

            return (T)parser;
        }
    }
}
