using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public static class _CodeDomObjectContextExtensions
    {
        #region CodeProperty

        public static CodeDomDefaultObjectContext AddCodeProperty(this CodeDomDefaultObjectContext context, ICodeProperty codeProperty)
        {
            Guard.NotNull(context, "context");
            return context.AddCustomValue("CodeProperty", codeProperty);
        }

        public static bool TryGetCodeProperty(this ICodeDomObjectContext context, out ICodeProperty codeProperty)
        {
            Guard.NotNull(context, "context");
            return context.CustomValues.TryGet<ICodeProperty>("CodeProperty", out codeProperty);
        }

        #endregion
    }
}
