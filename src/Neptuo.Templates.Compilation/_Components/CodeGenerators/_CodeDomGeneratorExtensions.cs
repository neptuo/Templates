using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
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

        public static CodeDomGenerator SetCodePropertyGenerator<T>(this CodeDomGenerator generator, ICodeDomPropertyGenerator propertyGenerator)
        {
            Guard.NotNull(generator, "generator");
            Guard.NotNull(propertyGenerator, "propertyGenerator");
            generator.SetCodePropertyGenerator(typeof(T), propertyGenerator);
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

        public static CodeDomGenerator SetStandartGenerators(this CodeDomGenerator generator, Type generatedViewBaseType, ComponentManagerDescriptor componentManagerDescriptor, Type requiredComponentType, IFieldNameProvider fieldNameProvider = null)
        {
            if (fieldNameProvider == null)
                fieldNameProvider = new SequenceFieldNameProvider();

            // Component generators.
            generator.SetCodeObjectGenerator<ComponentCodeObject>(new CodeDomComponentGenerator(fieldNameProvider, componentManagerDescriptor));
            generator.SetCodeObjectGenerator<CommentCodeObject>(new CodeDomCommentGenerator(fieldNameProvider, componentManagerDescriptor));
            generator.SetCodeObjectGenerator<PlainValueCodeObject>(new CodeDomPlainValueObjectGenerator());
            generator.SetCodeObjectGenerator<DependencyCodeObject>(new CodeDomDependencyObjectGenerator());
            //generator.SetCodeObjectGenerator<ComponentCodeObject>(new CodeDomExtensionObjectGenerator(fieldNameProvider, componentManager));
            generator.SetCodeObjectGenerator<LiteralCodeObject>(new CodeDomPlainValueObjectGenerator());
            generator.SetCodeObjectGenerator<RootCodeObject>(new CodeDomRootGenerator());

            // Property generators.
            generator.SetCodePropertyGenerator<ListAddCodeProperty>(new CodeDomListAddPropertyGenerator(requiredComponentType, componentManagerDescriptor));
            generator.SetCodePropertyGenerator<DictionaryAddCodeProperty>(new CodeDomDictionaryAddPropertyGenerator(requiredComponentType, componentManagerDescriptor));
            generator.SetCodePropertyGenerator<SetCodeProperty>(new CodeDomSetPropertyGenerator(requiredComponentType, componentManagerDescriptor));
            generator.SetCodePropertyGenerator<MethodInvokeCodeProperty>(new CodeDomMethodPropertyGenerator(requiredComponentType, componentManagerDescriptor));

            // Dependency generators.
            generator.SetDependencyProviderGenerator<object>(new CodeDomDependencyGenerator());

            // Base structure generator.
            CodeDomStructureGenerator structure = new CodeDomStructureGenerator();
            if (generatedViewBaseType != null)
                structure.BaseType = new CodeTypeReference(generatedViewBaseType);

            generator.SetBaseStructureGenerator(structure);
            return generator;
        }
    }
}
