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
        public class GenericContentTypeDescriptor
        {
            public Type Type { get; set; }
            public string TagNameProperty { get; set; }

            public GenericContentTypeDescriptor(Type type, string tagNameProperty)
            {
                Type = type;
                TagNameProperty = tagNameProperty;
            }

            public static GenericContentTypeDescriptor Create<T>(Expression<Func<T, string>> propertyGetter)
                where T : IControl
            {
                return new GenericContentTypeDescriptor(
                    typeof(T),
                    TypeHelper.PropertyName<T, string>(propertyGetter)
                );
            }
        }
    }
}
