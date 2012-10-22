using Neptuo.Web.Framework.Parser.ExtensionContent;
using Neptuo.Web.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Neptuo.Web.Framework.Compilation
{
    public partial class ExtensionValueParser : BaseParser, IValueParser
    {
        public bool Parse(string content, IContentParserContext context)
        {
            bool parsed = false;

            Helper helper = new Helper(context);
            helper.Parser.OnParsedItem = (e) => parsed = GenerateExtension(helper, e.ParsedItem);
            helper.Parser.Parse(content);

            return parsed;
        }

        private bool GenerateExtension(Helper helper, Extension extension)
        {
            Type controlType = helper.Registrator.GetExtension(extension.Namespace.ToLowerInvariant(), extension.Name.ToLowerInvariant());
            if (controlType == null)
                return false;

            ExtensionCodeObject codeObject = new ExtensionCodeObject(controlType);
            helper.Context.RootObject.AddProperty(codeObject);

            BindProperties(helper, codeObject, extension);
            return true;
        }

        private void BindProperties(Helper helper, ExtensionCodeObject codeObject, Extension extension)
        {
            HashSet<string> boundProperies = new HashSet<string>();
            PropertyInfo defaultProperty = ControlHelper.GetDefaultProperty(codeObject.Type);

            foreach (KeyValuePair<string, PropertyInfo> item in ControlHelper.GetProperties(codeObject.Type))
            {
                bool bound = false;
                string propertyName = item.Key.ToLowerInvariant();
                foreach (ExtensionAttribute attribute in extension.Attributes)
                {
                    if (propertyName == attribute.Name.ToLowerInvariant())
                    {
                        codeObject.PropertyInfo = new SetPropertyInfo(item.Value);
                        bool result = helper.Context.ParserService.ProcessValue(attribute.Value, new DefaultParserServiceContext(helper.Context.ServiceProvider, codeObject));
                        if (!result)
                            BindPropertyDefaultValue(codeObject, item.Value);

                        boundProperies.Add(propertyName);
                        bound = true;
                    }
                }

                if (!bound && item.Value != defaultProperty)
                {
                    BindPropertyDefaultValue(codeObject, item.Value);
                    boundProperies.Add(propertyName);
                    bound = true;
                }
            }

            if (defaultProperty != null && !boundProperies.Contains(defaultProperty.Name.ToLowerInvariant()))
            {
                if (!String.IsNullOrWhiteSpace(extension.DefaultAttributeValue))
                {
                    codeObject.PropertyInfo = new SetPropertyInfo(defaultProperty);
                    bool result = helper.Context.ParserService.ProcessValue(extension.DefaultAttributeValue, new DefaultParserServiceContext(helper.Context.ServiceProvider, codeObject));
                    if (!result)
                        BindPropertyDefaultValue(codeObject, defaultProperty);
                }
                else
                {
                    BindPropertyDefaultValue(codeObject, defaultProperty);
                }
            }
        }
    }
}
