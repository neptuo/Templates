using Neptuo.Web.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    partial class XmlContentParser
    {
        public class LiteralTypeDescriptor
        {
            public Type Type { get; set; }
            public string TextProperty { get; set; }

            public LiteralTypeDescriptor(Type type, string textProperty)
            {
                Type = type;
                TextProperty = textProperty;
            }

            public static LiteralTypeDescriptor Create<T>(Expression<Func<T, string>> propertyGetter)
                where T : IControl
            {
                return new LiteralTypeDescriptor(
                    typeof(T),
                    TypeHelper.PropertyName<T, string>(propertyGetter)
                );
            }
        }
    }
}
