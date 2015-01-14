using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public static class _CodeObjectExtensionContextExtensions
    {
        public static ObserverDictionary ObserverDictionary(this CodeObjectExtensionContext context)
        {
            Guard.NotNull(context, "context");
            return (ObserverDictionary)context.CustomValues["ObserverDictionary"];
        }

        public static CodeObjectExtensionContext ObserverDictionary(this CodeObjectExtensionContext context, ObserverDictionary observerDictionary)
        {
            Guard.NotNull(context, "context");
            context.CustomValues["ObserverDictionary"] = observerDictionary;
            return context;
        }
    }
}
