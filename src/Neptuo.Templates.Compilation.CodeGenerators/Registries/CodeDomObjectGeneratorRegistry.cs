using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Registry for <see cref="ICodeDomObjectGenerator"/> by type of code object.
    /// </summary>
    public class CodeDomObjectGeneratorRegistry : ICodeDomObjectGenerator
    {
        private readonly Dictionary<Type, ICodeDomObjectGenerator> storage = new Dictionary<Type, ICodeDomObjectGenerator>();

        /// <summary>
        /// Maps <paramref name="generator"/> to process code objects of type <paramref name="codeObjectType"/>.
        /// </summary>
        /// <param name="codeObjectType">Type of code object to process by <paramref name="generator"/>.</param>
        /// <param name="generator">Generator to process code objects of type <paramref name="codeObjectType"/>.</param>
        public CodeDomObjectGeneratorRegistry AddGenerator(Type codeObjectType, ICodeDomObjectGenerator generator)
        {
            Guard.NotNull(codeObjectType, "codeObjectType");
            Guard.NotNull(generator, "generator");
            storage[codeObjectType] = generator;
            return this;
        }

        public CodeExpression Generate(ICodeDomContext context, ICodeObject codeObject)
        {
            Guard.NotNull(context, "context");
            Guard.NotNull(codeObject, "codeObject");
            Type codeObjectType = codeObject.GetType();

            ICodeDomObjectGenerator generator;
            if(!storage.TryGetValue(codeObjectType, out generator))
            {
                context.AddError("Missing generator for code object of type '{0}'.", codeObjectType.FullName);
                return null;
            }

            return generator.Generate(context, codeObject);
        }
    }
}
