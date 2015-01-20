using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Registry for <see cref="ICodeDomPropertyTypeGenerator"/> by type of unbound property.
    /// </summary>
    public class CodeDomPropertyTypeGeneratorRegistry : ICodeDomPropertyTypeGenerator
    {
        private readonly Dictionary<Type, ICodeDomPropertyTypeGenerator> storage = new Dictionary<Type, ICodeDomPropertyTypeGenerator>();
        private Func<PropertyInfo, ICodeDomPropertyTypeGenerator> onSearchGenerator = o => new NullGenerator();

        /// <summary>
        /// Maps <paramref name="generator"/> to process undound properties of type <paramref name="codeObjectType"/>.
        /// </summary>
        /// <param name="codeObjectType">Type of undound property to process by <paramref name="generator"/>.</param>
        /// <param name="generator">Generator to process undound properties of type <paramref name="codeObjectType"/>.</param>
        public CodeDomPropertyTypeGeneratorRegistry AddGenerator(Type codeObjectType, ICodeDomPropertyTypeGenerator generator)
        {
            Guard.NotNull(codeObjectType, "codeObjectType");
            Guard.NotNull(generator, "generator");
            storage[codeObjectType] = generator;
            return this;
        }

        /// <summary>
        /// Adds <paramref name="searchHandler"/> to be executed when generator was not found.
        /// </summary>
        /// <param name="searchHandler">Generator provider method.</param>
        public CodeDomPropertyTypeGeneratorRegistry AddSearchHandler(Func<PropertyInfo, ICodeDomPropertyTypeGenerator> searchHandler)
        {
            Guard.NotNull(searchHandler, "searchHandler");
            onSearchGenerator += searchHandler;
            return this;
        }

        public ICodeDomPropertyTypeResult Generate(ICodeDomContext context, PropertyInfo propertyInfo)
        {
            Guard.NotNull(context, "context");
            Guard.NotNull(propertyInfo, "propertyInfo");
            Type propertyType = propertyInfo.PropertyType;

            ICodeDomPropertyTypeGenerator generator;
            if (!storage.TryGetValue(propertyType, out generator))
                generator = onSearchGenerator(propertyInfo);

            return generator.Generate(context, propertyInfo);
        }

        private class NullGenerator : ICodeDomPropertyTypeGenerator
        {
            public ICodeDomPropertyTypeResult Generate(ICodeDomContext context, PropertyInfo propertyInfo)
            {
                return new CodeDomDefaultPropertyTypeResult();
            }
        }
    }
}
