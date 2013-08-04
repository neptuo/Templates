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
        public static void Register(CodeDomGenerator generator)
        {
            generator.SetCodeObjectExtension(typeof(ControlCodeObject), new CodeDomComponentGenerator());
            generator.SetCodeObjectExtension(typeof(PlainValueCodeObject), new CodeDomPlainValueObjectGenerator());
            generator.SetCodeObjectExtension(typeof(LocalFieldCodeObject), new CodeDomLocalFieldObjectGenerator());
            generator.SetCodeObjectExtension(typeof(DependencyCodeObject), new CodeDomDependencyObjectGenerator());


            generator.SetPropertyDescriptorExtension(typeof(ListAddPropertyDescriptor), new CodeDomListAddPropertyGenerator());
            generator.SetPropertyDescriptorExtension(typeof(SetPropertyDescriptor), new CodeDomSetPropertyGenerator());
            generator.SetPropertyDescriptorExtension(typeof(MethodInvokePropertyDescriptor), new CodeDomMethodPropertyGenerator());


            generator.SetDependencyProviderExtension(typeof(object), new CodeDomDependencyGenerator());


            generator.SetBaseStructureExtension(new CodeDomStructureGenerator());
        }
    }
}
