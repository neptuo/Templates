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
    /// Registry for <see cref="XICodeDomPropertyGenerator"/> by type of code object.
    /// </summary>
    public class CodeDomPropertyGeneratorRegistry : ICodeDomPropertyGenerator
    {
        private readonly Dictionary<Type, ICodeDomPropertyGenerator> storage = new Dictionary<Type, ICodeDomPropertyGenerator>();

        /// <summary>
        /// Maps <paramref name="generator"/> to process code properties of type <paramref name="codeObjectType"/>.
        /// </summary>
        /// <param name="codeObjectType">Type of code property to process by <paramref name="generator"/>.</param>
        /// <param name="generator">Generator to process code properties of type <paramref name="codeObjectType"/>.</param>
        public CodeDomPropertyGeneratorRegistry AddGenerator(Type codeObjectType, ICodeDomPropertyGenerator generator)
        {
            Guard.NotNull(codeObjectType, "codeObjectType");
            Guard.NotNull(generator, "generator");
            storage[codeObjectType] = generator;
            return this;
        }

        public CodeExpression Generate(ICodeDomContext context, ICodeProperty codeProperty)
        {
            Guard.NotNull(context, "context");
            Guard.NotNull(codeProperty, "codeProperty");
            Type codePropertyType = codeProperty.GetType();

            ICodeDomPropertyGenerator generator;
            if(!storage.TryGetValue(codePropertyType, out generator))
            {
                context.AddError("Missing generator for code property of type '{0}'.", codePropertyType.FullName);
                return null;
            }

            return generator.Generate(context, codeProperty);
        }
    }
}
