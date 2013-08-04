using Neptuo.Linq.Expressions;
using Neptuo.Xml;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Neptuo.Templates.Compilation.Parsers
{
    public abstract class BaseControlBuilder : BaseComponentBuilder
    {
        protected abstract Type GetControlType(XmlElement element);

        protected override IComponentCodeObject CreateCodeObject(IBuilderContext context, XmlElement element)
        {
            return new ComponentCodeObject(GetControlType(element));
        }

        protected override IComponentDefinition GetComponentDefinition(IBuilderContext context, IComponentCodeObject codeObject, XmlElement element)
        {
            return new TypeComponentDefinition(GetControlType(element));
        }

        protected override void ProcessUnboundAttribute(IBuilderContext context, IComponentCodeObject codeObject, XmlAttribute attribute)
        {
            ITypeCodeObject typeCodeObject = codeObject as ITypeCodeObject;
            if (typeCodeObject != null)
                BindAttributeCollection(context, typeCodeObject, codeObject, attribute.LocalName, attribute.Value);
            else
                throw new NotImplementedException("Can't process unbound attributes!");
        }

        protected override IPropertyDescriptor CreateListAddPropertyDescriptor(IPropertyInfo propertyInfo)
        {
            return new ListAddPropertyDescriptor(propertyInfo);
        }

        protected override IPropertyDescriptor CreateSetPropertyDescriptor(IPropertyInfo propertyInfo)
        {
            return new SetPropertyDescriptor(propertyInfo);
        }

        public static bool BindAttributeCollection(IBuilderContext context, ITypeCodeObject typeCodeObject, IPropertiesCodeObject propertiesCodeObject, string name, string value)
        {
            MethodInfo method = typeCodeObject.Type.GetMethod(TypeHelper.MethodName<IAttributeCollection, string, string>(a => a.SetAttribute));
            MethodInvokePropertyDescriptor propertyDescriptor = new MethodInvokePropertyDescriptor(method);
            propertyDescriptor.SetValue(new PlainValueCodeObject(name));

            bool result = context.ParserContext.ParserService.ProcessValue(
                value,
                new DefaultParserServiceContext(context.ParserContext.DependencyProvider, propertyDescriptor, context.ParserContext.Errors)
            );
            if (result)
                propertiesCodeObject.Properties.Add(propertyDescriptor);

            //TODO: Else NOT result?
            return result;
        }

    }
}
