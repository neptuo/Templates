using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomAttributeGeneratorRegistry : ICodeDomAttributeGenerator
    {
        private readonly Dictionary<Type, ICodeDomAttributeGenerator> storage = new Dictionary<Type, ICodeDomAttributeGenerator>();
        private Func<Type, ICodeDomAttributeGenerator> onSearchGenerator;

        /// <summary>
        /// Maps <paramref name="generator"/> to process attributes of type <paramref name="attributeType"/>.
        /// </summary>
        /// <param name="attributeType">Type of attribute to process by <paramref name="generator"/>.</param>
        /// <param name="generator">Generator to process attributes of type <paramref name="attributeType"/>.</param>
        public CodeDomAttributeGeneratorRegistry AddGenerator(Type attributeType, ICodeDomAttributeGenerator generator)
        {
            Guard.NotNull(attributeType, "attribute");
            Guard.NotNull(generator, "generator");
            storage[attributeType] = generator;
            return this;
        }

        /// <summary>
        /// Adds <paramref name="searchHandler"/> to be executed when generator was not found.
        /// </summary>
        /// <param name="searchHandler">Generator provider method.</param>
        public CodeDomAttributeGeneratorRegistry AddSearchHandler(Func<Type, ICodeDomAttributeGenerator> searchHandler)
        {
            Guard.NotNull(searchHandler, "searchHandler");
            onSearchGenerator += searchHandler;
            return this;
        }

        public ICodeDomAttributeResult Generate(ICodeDomContext context, Attribute attribute)
        {
            Guard.NotNull(context, "context");
            Guard.NotNull(attribute, "attribute");
            Type attributeType = attribute.GetType();

            ICodeDomAttributeGenerator generator;
            if (!storage.TryGetValue(attributeType, out generator) && onSearchGenerator != null)
                generator = onSearchGenerator(attributeType);

            if (generator == null)
                return new CodeDomDefaultAttributeResult();

            return generator.Generate(context, attribute);
        }
    }
}
