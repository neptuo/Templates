using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public static class _IXmlContentBuilderContextExtensions
    {
        #region ComponentCodeObject

        public static IXmlContentBuilderContext CodeObject(this IXmlContentBuilderContext context, ICodeObject codeObject)
        {
            Guard.NotNull(context, "context");
            context.CustomValues["CodeObject"] = codeObject;
            return context;
        }

        public static ICodeObject CodeObject(this IXmlContentBuilderContext context)
        {
            Guard.NotNull(context, "context");
            return (ICodeObject)context.CustomValues["CodeObject"];
        }

        public static IPropertiesCodeObject CodeObjectAsProperties(this IXmlContentBuilderContext context)
        {
            Guard.NotNull(context, "context");
            return (IPropertiesCodeObject)CodeObject(context);
        }

        public static IObserversCodeObject CodeObjectAsObservers(this IXmlContentBuilderContext context)
        {
            Guard.NotNull(context, "context");
            return (IObserversCodeObject)CodeObject(context);
        }

        #endregion

        #region ComponentDescriptor

        public static IComponentDescriptor ComponentDescriptor(this IXmlContentBuilderContext context)
        {
            Guard.NotNull(context, "context");
            return (IComponentDescriptor)context.CustomValues["ComponentDescriptor"];
        }

        public static IXmlContentBuilderContext ComponentDescriptor(this IXmlContentBuilderContext context, IComponentDescriptor componentDescriptor)
        {
            Guard.NotNull(context, "context");
            context.CustomValues["ComponentDescriptor"] = componentDescriptor;
            return context;
        }

        #endregion

        #region BindPropertiesContext

        public static XmlBindContentPropertiesContext BindPropertiesContext(this IXmlContentBuilderContext context)
        {
            Guard.NotNull(context, "context");
            return (XmlBindContentPropertiesContext)context.CustomValues["BindPropertiesContext"];
        }

        public static IXmlContentBuilderContext BindPropertiesContext(this IXmlContentBuilderContext context, XmlBindContentPropertiesContext bindPropertiesContext)
        {
            Guard.NotNull(context, "context");
            context.CustomValues["BindPropertiesContext"] = bindPropertiesContext;
            return context;
        }

        #endregion

        #region DefaultProperty

        public static IPropertyInfo DefaultProperty(this IXmlContentBuilderContext context)
        {
            Guard.NotNull(context, "context");
            return (IPropertyInfo)context.CustomValues["DefaultProperty"];
        }

        public static IXmlContentBuilderContext DefaultProperty(this IXmlContentBuilderContext context, IPropertyInfo defaultProperty)
        {
            Guard.NotNull(context, "context");
            context.CustomValues["DefaultProperty"] = defaultProperty;
            return context;
        }

        #endregion
    }
}
