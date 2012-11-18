using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Compilation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators
{
    public class DefaultCodeGeneratorService : ICodeGeneratorService
    {
        private Dictionary<string, ICodeGenerator> generators = new Dictionary<string, ICodeGenerator>();

        public void AddGenerator(string name, ICodeGenerator generator)
        {
            generators[name] = generator;
        }

        public bool GeneratedCode(string name, IPropertyDescriptor propertyDescriptor, ICodeGeneratorServiceContext context)
        {
            if (generators.ContainsKey(name))
            {
                IDependencyContainer provider = context.DependencyProvider.CreateChildContainer();
                //if (provider.Resolve<StorageProvider>() == null) TODO: Solve if is registered!
                provider.RegisterInstance<StorageProvider>(new StorageProvider());

                return generators[name].ProcessTree(propertyDescriptor, new DefaultCodeGeneratorContext(context.Output, this, provider));
            }

            return false;
        }
    }
}
