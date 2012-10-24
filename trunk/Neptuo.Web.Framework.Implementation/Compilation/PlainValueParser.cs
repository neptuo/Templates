using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Utils;
using TypeConverter = Neptuo.Web.Framework.Utils.TypeConverter;
using Neptuo.Web.Framework.Compilation.CodeObjects;

namespace Neptuo.Web.Framework.Compilation
{
    public class PlainValueParser : BaseParser, IValueParser
    {
        public bool Parse(string content, IValueParserContext context)
        {
            if (TypeConverter.CanConvert(context.PropertyDescriptor.Property.PropertyType))
            {
                context.PropertyDescriptor.SetValue(
                    new PlainValueCodeObject(
                        TypeConverter.Convert(content, context.PropertyDescriptor.Property.PropertyType)
                    )
                );
                return true;
            }
            //else
            //{
            //    BindPropertyDefaultValue(context, codeObject);
            //}

            return false;
        }
    }
}
