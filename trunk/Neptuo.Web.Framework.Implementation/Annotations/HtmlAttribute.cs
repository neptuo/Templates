using Neptuo.Web.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Annotations
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class HtmlAttribute : Attribute
    {
        public string TagName { get; set; }
        public bool IsSelfClosing { get; set; }

        public HtmlAttribute(string tagName)
        {
            TagName = tagName;
        }

        public HtmlAttribute(string tagName, bool isSelfClosing)
            : this(tagName)
        {
            IsSelfClosing = isSelfClosing;
        }

        public static HtmlAttribute GetAttribute(Type type)
        {
            return ReflectionHelper.GetAttribute<HtmlAttribute>(type);
        }
    }
}
