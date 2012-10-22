using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Utils;
using TypeConverter = Neptuo.Web.Framework.Utils.TypeConverter;

namespace Neptuo.Web.Framework.Compilation
{
    public class PlainValueParser : BaseParser, IValueParser
    {
        public bool Parse(string content, IValueParserContext context)
        {
            XmlContentParser.CodeObject codeObject = context.RootObject as XmlContentParser.CodeObject;
            if (codeObject != null)
            {
                if (TypeConverter.CanConvert(context.PropertyDescriptor.Property.PropertyType))
                {
                    codeObject.AddProperty(
                        new XmlContentParser.PlainValueCodeObject(
                            TypeConverter.Convert(content, codeObject.PropertyInfo.RequiredType)
                        )
                    );
                }
                //else
                //{
                //    BindPropertyDefaultValue(context, codeObject);
                //}

                return true;
            }
            return false;
        }
    }
}
