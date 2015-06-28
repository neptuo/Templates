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

        public static IContentBuilderContext CodeObject(this IContentBuilderContext context, ICodeObject codeObject)
        {
            Ensure.NotNull(context, "context");
            context.CustomValues["CodeObject"] = codeObject;
            return context;
        }

        public static ICodeObject CodeObject(this IContentBuilderContext context)
        {
            Ensure.NotNull(context, "context");
            return (ICodeObject)context.CustomValues["CodeObject"];
        }

        public static IPropertiesCodeObject CodeObjectAsProperties(this IContentBuilderContext context)
        {
            Ensure.NotNull(context, "context");
            return (IPropertiesCodeObject)CodeObject(context);
        }

        public static IObserversCodeObject CodeObjectAsObservers(this IContentBuilderContext context)
        {
            Ensure.NotNull(context, "context");
            return (IObserversCodeObject)CodeObject(context);
        }

        #endregion

        #region ComponentDescriptor

        public static IXComponentDescriptor ComponentDescriptor(this IContentBuilderContext context)
        {
            Ensure.NotNull(context, "context");
            return (IXComponentDescriptor)context.CustomValues["ComponentDescriptor"];
        }

        public static IContentBuilderContext ComponentDescriptor(this IContentBuilderContext context, IXComponentDescriptor componentDescriptor)
        {
            Ensure.NotNull(context, "context");
            context.CustomValues["ComponentDescriptor"] = componentDescriptor;
            return context;
        }

        #endregion

        #region BindPropertiesContext

        public static BindContentPropertiesContext BindPropertiesContext(this IContentBuilderContext context)
        {
            Ensure.NotNull(context, "context");
            return (BindContentPropertiesContext)context.CustomValues["BindPropertiesContext"];
        }

        public static IContentBuilderContext BindPropertiesContext(this IContentBuilderContext context, BindContentPropertiesContext bindPropertiesContext)
        {
            Ensure.NotNull(context, "context");
            context.CustomValues["BindPropertiesContext"] = bindPropertiesContext;
            return context;
        }

        #endregion

        #region DefaultProperty

        public static IPropertyInfo DefaultProperty(this IContentBuilderContext context)
        {
            Ensure.NotNull(context, "context");
            return (IPropertyInfo)context.CustomValues["DefaultProperty"];
        }

        public static IContentBuilderContext DefaultProperty(this IContentBuilderContext context, IPropertyInfo defaultProperty)
        {
            Ensure.NotNull(context, "context");
            context.CustomValues["DefaultProperty"] = defaultProperty;
            return context;
        }

        #endregion
    }
}
