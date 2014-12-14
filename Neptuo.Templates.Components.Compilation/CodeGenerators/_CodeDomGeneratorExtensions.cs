using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public static class _CodeDomGeneratorExtensions
    {
        public static CodeDomGenerator SetCodeObjectGenerator<T>(this CodeDomGenerator generator, ICodeDomComponentGenerator componentGenerator)
        {
            Guard.NotNull(generator, "generator");
            Guard.NotNull(componentGenerator, "codeObjectGenerator");
            generator.SetCodeObjectGenerator(typeof(T), componentGenerator);
            return generator;
        }

        public static CodeDomGenerator SetPropertyDescriptorGenerator<T>(this CodeDomGenerator generator, ICodeDomPropertyGenerator propertyGenerator)
        {
            Guard.NotNull(generator, "generator");
            Guard.NotNull(propertyGenerator, "propertyGenerator");
            generator.SetPropertyDescriptorGenerator(typeof(T), propertyGenerator);
            return generator;
        }

        public static CodeDomGenerator SetDependencyProviderGenerator<T>(this CodeDomGenerator generator, ICodeDomDependencyGenerator dependencyGenerator)
        {
            Guard.NotNull(generator, "generator");
            Guard.NotNull(dependencyGenerator, "dependencyGenerator");
            generator.SetDependencyProviderGenerator(typeof(T), dependencyGenerator);
            return generator;
        }

        public static CodeDomGenerator SetAttributeGenerator<T>(this CodeDomGenerator generator, ICodeDomAttributeGenerator attributeGenerator)
        {
            Guard.NotNull(generator, "generator");
            Guard.NotNull(attributeGenerator, "attributeGenerator");
            generator.SetAttributeGenerator(typeof(T), attributeGenerator);
            return generator;
        }

        public static CodeDomGenerator SetStandartGenerators(this CodeDomGenerator generator, IFieldNameProvider fieldNameProvider = null)
        {
            if (fieldNameProvider == null)
                fieldNameProvider = new SequenceFieldNameProvider();

            // Component generators.
            generator.SetCodeObjectGenerator<ComponentCodeObject>(new CodeDomComponentGenerator(fieldNameProvider));
            generator.SetCodeObjectGenerator<CommentCodeObject>(new CodeDomCommentGenerator(fieldNameProvider));
            generator.SetCodeObjectGenerator<PlainValueCodeObject>(new CodeDomPlainValueObjectGenerator());
            generator.SetCodeObjectGenerator<DependencyCodeObject>(new CodeDomDependencyObjectGenerator());
            generator.SetCodeObjectGenerator<ExtensionCodeObject>(new CodeDomExtensionObjectGenerator(fieldNameProvider));
            generator.SetCodeObjectGenerator<LiteralCodeObject>(new CodeDomPlainValueObjectGenerator());
            generator.SetCodeObjectGenerator<RootCodeObject>(new CodeDomRootGenerator());

            // Property generators.
            generator.SetPropertyDescriptorGenerator<ListAddPropertyDescriptor>(new CodeDomListAddPropertyGenerator());
            generator.SetPropertyDescriptorGenerator<DictionaryAddPropertyDescriptor>(new CodeDomDictionaryAddPropertyGenerator());
            generator.SetPropertyDescriptorGenerator<SetPropertyDescriptor>(new CodeDomSetPropertyGenerator());
            generator.SetPropertyDescriptorGenerator<MethodInvokePropertyDescriptor>(new CodeDomMethodPropertyGenerator());

            // Dependency generators.
            generator.SetDependencyProviderGenerator<object>(new CodeDomDependencyGenerator());

            // Base structure generator.
            generator.SetBaseStructureGenerator(new CodeDomStructureGenerator());

            return generator;
        }
    }
}
