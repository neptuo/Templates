
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public static class _CodeDomPropertyContextExtensions
    {
        #region FieldType

        public static CodeDomDefaultPropertyContext AddFieldType(this CodeDomDefaultPropertyContext context, Type fieldType)
        {
            Guard.NotNull(context, "context");
            return context.AddCustomValue("FieldType", fieldType);
        }

        public static bool TryGetFieldType(this ICodeDomPropertyContext context, out Type fieldType)
        {
            Guard.NotNull(context, "context");
            return context.CustomValues.TryGet<Type>("FieldType", out fieldType);
        }

        #endregion

    }
}
