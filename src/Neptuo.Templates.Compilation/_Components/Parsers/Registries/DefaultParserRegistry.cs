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

        public DefaultParserRegistry AddRegistry<T>(T parser)
        {
            Guard.NotNull(parser, "parser");
            storage[typeof(T)] = parser;
            return this;
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
