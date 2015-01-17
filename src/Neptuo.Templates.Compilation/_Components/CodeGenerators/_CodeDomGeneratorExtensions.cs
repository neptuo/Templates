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
        public static XCodeDomGenerator SetCodeObjectGenerator<T>(this XCodeDomGenerator generator, ICodeDomComponentGenerator componentGenerator)
        {
            Guard.NotNull(generator, "generator");
            Guard.NotNull(componentGenerator, "codeObjectGenerator");
            generator.SetCodeObjectGenerator(typeof(T), componentGenerator);
            return generator;
        }

        public static XCodeDomGenerator SetCodePropertyGenerator<T>(this XCodeDomGenerator generator, XICodeDomPropertyGenerator propertyGenerator)
        {
            Guard.NotNull(generator, "generator");
            Guard.NotNull(propertyGenerator, "propertyGenerator");
            generator.SetCodePropertyGenerator(typeof(T), propertyGenerator);
            return generator;
        }

        public static XCodeDomGenerator SetDependencyProviderGenerator<T>(this XCodeDomGenerator generator, XICodeDomDependencyGenerator dependencyGenerator)
        {
            Guard.NotNull(generator, "generator");
            Guard.NotNull(dependencyGenerator, "dependencyGenerator");
            generator.SetDependencyProviderGenerator(typeof(T), dependencyGenerator);
            return generator;
        }

        public static XCodeDomGenerator SetAttributeGenerator<T>(this XCodeDomGenerator generator, ICodeDomAttributeGenerator attributeGenerator)
        {
            Guard.NotNull(generator, "generator");
            Guard.NotNull(attributeGenerator, "attributeGenerator");
            generator.SetAttributeGenerator(typeof(T), attributeGenerator);
            return generator;
        }

        public static XCodeDomGenerator SetStandartGenerators(this XCodeDomGenerator generator, Type generatedViewBaseType, ComponentManagerDescriptor componentManagerDescriptor, Type requiredComponentType, IFieldNameProvider fieldNameProvider = null)
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
            generator.SetCodePropertyGenerator<ListAddCodeProperty>(new XCodeDomListAddPropertyGenerator(requiredComponentType, componentManagerDescriptor));
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
