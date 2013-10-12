using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public static class CODEDOMREGISTEREXTENSIONS
    {
        public static void RegisterStandartCodeGenerators(this CodeDomGenerator generator, IFieldNameProvider fieldNameProvider = null)
        {
            if (fieldNameProvider == null)
                fieldNameProvider = new SequenceFieldNameProvider();

            generator.SetCodeObjectGenerator(typeof(ComponentCodeObject), new CodeDomComponentGenerator(fieldNameProvider));
            generator.SetCodeObjectGenerator(typeof(PlainValueCodeObject), new CodeDomPlainValueObjectGenerator());
            generator.SetCodeObjectGenerator(typeof(DependencyCodeObject), new CodeDomDependencyObjectGenerator());
            generator.SetCodeObjectGenerator(typeof(ExtensionCodeObject), new CodeDomExtensionObjectGenerator(fieldNameProvider));

            generator.SetPropertyDescriptorGenerator(typeof(ListAddPropertyDescriptor), new CodeDomListAddPropertyGenerator());
            generator.SetPropertyDescriptorGenerator(typeof(SetPropertyDescriptor), new CodeDomSetPropertyGenerator());
            generator.SetPropertyDescriptorGenerator(typeof(MethodInvokePropertyDescriptor), new CodeDomMethodPropertyGenerator());


            generator.SetDependencyProviderGenerator(typeof(object), new CodeDomDependencyGenerator());


            generator.SetBaseStructureGenerator(new CodeDomStructureGenerator());
        }
    }
}
