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
        #region ComponentCodeObject

        public static IComponentCodeObject ComponentCodeObject(this IContentBuilderContext context)
        {
            Guard.NotNull(context, "context");
            return (IComponentCodeObject)context.CustomValues["ComponentCodeObject"];
        }

        public static IContentBuilderContext ComponentCodeObject(this IContentBuilderContext context, IComponentCodeObject codeObject)
        {
            Guard.NotNull(context, "context");
            context.CustomValues["ComponentCodeObject"] = codeObject;
            return context;
        }

        #endregion

        #region ComponentDescriptor

        public static IComponentDescriptor ComponentDescriptor(this IContentBuilderContext context)
        {
            Guard.NotNull(context, "context");
            return (IComponentDescriptor)context.CustomValues["ComponentDescriptor"];
        }

        public static IContentBuilderContext ComponentDescriptor(this IContentBuilderContext context, IComponentDescriptor componentDescriptor)
        {
            Guard.NotNull(context, "context");
            context.CustomValues["ComponentDescriptor"] = componentDescriptor;
            return context;
        }

        #endregion

        #region BindPropertiesContext

        public static BindPropertiesContext BindPropertiesContext(this IContentBuilderContext context)
        {
            Guard.NotNull(context, "context");
            return (BindPropertiesContext)context.CustomValues["BindPropertiesContext"];
        }

        public static IContentBuilderContext BindPropertiesContext(this IContentBuilderContext context, BindPropertiesContext bindPropertiesContext)
        {
            Guard.NotNull(context, "context");
            context.CustomValues["BindPropertiesContext"] = bindPropertiesContext;
            return context;
        }

        #endregion

        #region DefaultProperty

        public static IPropertyInfo DefaultProperty(this IContentBuilderContext context)
        {
            Guard.NotNull(context, "context");
            return (IPropertyInfo)context.CustomValues["DefaultProperty"];
        }

        public static IContentBuilderContext DefaultProperty(this IContentBuilderContext context, IPropertyInfo defaultProperty)
        {
            Guard.NotNull(context, "context");
            context.CustomValues["DefaultProperty"] = defaultProperty;
            return context;
        }

        #endregion
    }
}
