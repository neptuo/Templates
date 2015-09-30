using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public static class _IContentBuilderContextExtensions
    {
        public static IObserversCodeObject CodeObjectAsObservers(this IContentBuilderContext context)
        {
            Ensure.NotNull(context, "context");
            return (IObserversCodeObject)context.CodeObject();
        }
    }
}
