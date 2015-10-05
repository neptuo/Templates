using Neptuo.Identifiers;
using Neptuo.Models.Features;
using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Component generator which delegates execution to sub generator.
    /// </summary>
    public class CodeDomDelegatingObjectGenerator : CodeDomObjectGeneratorBase<IComponentCodeObject>
    {
        private readonly ICodeDomObjectGenerator controlGenerator;
        private readonly ICodeDomObjectGenerator extensionGenerator;
        private readonly ICodeDomObjectGenerator objectGenerator;

        public CodeDomDelegatingObjectGenerator(IUniqueNameProvider nameProvider)
        {
            controlGenerator = new CodeDomControlObjectGenerator(nameProvider);
            extensionGenerator = new CodeDomValueExtensionObjectGenerator(nameProvider);
            objectGenerator = new CodeDomComponentObjectGenerator(nameProvider);
        }

        protected override ICodeDomObjectResult Generate(ICodeDomObjectContext context, IComponentCodeObject codeObject)
        {
            ITypeCodeObject typeObject;
            if (codeObject.TryWith(out typeObject))
            {
                if (typeof(IValueExtension).IsAssignableFrom(typeObject.Type))
                    return extensionGenerator.Generate(context, codeObject);
                else if (typeof(IControl).IsAssignableFrom(typeObject.Type))
                    return controlGenerator.Generate(context, codeObject);
            }

            return objectGenerator.Generate(context, codeObject);
        }
    }
}
