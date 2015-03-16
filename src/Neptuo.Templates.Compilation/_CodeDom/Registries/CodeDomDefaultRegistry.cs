using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Default implementation of <see cref="ICodeDomRegistry"/>.
    /// </summary>
    public class CodeDomDefaultRegistry : ICodeDomRegistry
    {
        private readonly Dictionary<Type, object> storage = new Dictionary<Type, object>();

        public CodeDomDefaultRegistry AddRegistry<T>(T generator)
        {
            Ensure.NotNull(generator, "generator");
            storage[typeof(T)] = generator;
            return this;
        }

        public T With<T>()
        {
            Type generatorType = typeof(T);
            object generator;
            if (!storage.TryGetValue(generatorType, out generator))
                throw Ensure.Exception.ArgumentOutOfRange("T", "Unnable to resolve generator of type '{0}' from code dom registry.", generatorType.FullName);

            return (T)generator;
        }
    }
}
