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
        private FuncList<Type, ICodeDomAttributeGenerator> onSearchGenerator = new FuncList<Type,ICodeDomAttributeGenerator>();

        /// <summary>
        /// Maps <paramref name="generator"/> to process attributes of type <paramref name="attributeType"/>.
        /// </summary>
        /// <param name="attributeType">Type of attribute to process by <paramref name="generator"/>.</param>
        /// <param name="generator">Generator to process attributes of type <paramref name="attributeType"/>.</param>
        public CodeDomAttributeGeneratorRegistry AddGenerator(Type attributeType, ICodeDomAttributeGenerator generator)
        {
            Ensure.NotNull(attributeType, "attribute");
            Ensure.NotNull(generator, "generator");
            storage[attributeType] = generator;
            return this;
        }

        /// <summary>
        /// Adds <paramref name="searchHandler"/> to be executed when generator was not found.
        /// </summary>
        /// <param name="searchHandler">Generator provider method.</param>
        public CodeDomAttributeGeneratorRegistry AddSearchHandler(Func<Type, ICodeDomAttributeGenerator> searchHandler)
        {
            Ensure.NotNull(searchHandler, "searchHandler");
            onSearchGenerator.Add(searchHandler);
            return this;
        }

        public ICodeDomAttributeResult Generate(ICodeDomContext context, Attribute attribute)
        {
            Ensure.NotNull(context, "context");
            Ensure.NotNull(attribute, "attribute");
            Type attributeType = attribute.GetType();

            ICodeDomAttributeGenerator generator;
            if (!storage.TryGetValue(attributeType, out generator) && onSearchGenerator != null)
                generator = onSearchGenerator.Execute(attributeType);

            if (generator == null)
                return new CodeDomDefaultAttributeResult();

            return generator.Generate(context, attribute);
        }
    }
}
