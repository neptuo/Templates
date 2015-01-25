using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public static class _CodeDomObjectContextExtensions
    {
        #region PropertyTarget

        public static CodeDomDefaultObjectContext AddPropertyTarget(this CodeDomDefaultObjectContext context, CodeExpression propertyTarget)
        {
            Guard.NotNull(context, "context");
            return context.AddCustomValue("PropertyTarget", propertyTarget);
        }

        public static bool TryGetPropertyTarget(this ICodeDomObjectContext context, out CodeExpression propertyTarget)
        {
            Guard.NotNull(context, "context");
            return context.CustomValues.TryGet("PropertyTarget", out propertyTarget);
        }

        #endregion

        #region CodeProperty

        public static CodeDomDefaultObjectContext AddCodeProperty(this CodeDomDefaultObjectContext context, ICodeProperty codeProperty)
        {
            Guard.NotNull(context, "context");
            return context.AddCustomValue("CodeProperty", codeProperty);
        }

        public static bool TryGetCodeProperty(this ICodeDomObjectContext context, out ICodeProperty codeProperty)
        {
            Guard.NotNull(context, "context");
            return context.CustomValues.TryGet("CodeProperty", out codeProperty);
        }

        #endregion

        #region ObserverTarget

        public static CodeDomDefaultObjectContext AddObserverTarget(this CodeDomDefaultObjectContext context, string variableName)
        {
            Guard.NotNull(context, "context");
            return context.AddCustomValue("ObserverTarget", variableName);
        }

        public static bool TryGetObserverTarget(this ICodeDomObjectContext context, out string variableName)
        {
            Guard.NotNull(context, "context");
            return context.CustomValues.TryGet("ObserverTarget", out variableName);
        }

        #endregion
    }
}
