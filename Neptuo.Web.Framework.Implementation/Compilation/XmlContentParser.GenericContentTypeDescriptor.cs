using System;
using System.Collections.Generic;
using System.Linq;
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
        }
    }
}
