using Neptuo.Templates.Compilation.CodeGenerators.Extensions.CodeDom;
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
            generator.SetCodeObjectExtension(typeof(ControlCodeObject), new ComponentCodeObjectExtension());
            generator.SetCodeObjectExtension(typeof(PlainValueCodeObject), new PlainValueCodeObjectExtension());
            generator.SetCodeObjectExtension(typeof(LocalFieldCodeObject), new LocalFieldCodeObjectExtension());
            generator.SetCodeObjectExtension(typeof(DependencyCodeObject), new DependencyCodeObjectExtension());


            generator.SetPropertyDescriptorExtension(typeof(ListAddPropertyDescriptor), new ListAddPropertyDescriptorExtension());
            generator.SetPropertyDescriptorExtension(typeof(SetPropertyDescriptor), new SetPropertyDescriptorExtension());
            generator.SetPropertyDescriptorExtension(typeof(MethodInvokePropertyDescriptor), new MethodInvokePropertyDescriptorExtension());


            generator.SetDependencyProviderExtension(typeof(object), new DefaultDependencyProviderExtension());


            generator.SetBaseStructureExtension(new BaseStructureExtension());
        }
    }
}
