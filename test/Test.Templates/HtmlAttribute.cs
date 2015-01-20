using Neptuo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Templates
{
    /// <summary>
    /// Provides metadata about target html tag.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class HtmlAttribute : Attribute
    {
        /// <summary>
        /// Tag name.
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// Whether html tag is self closing.
        /// </summary>
        public bool IsSelfClosing { get; set; }

        public HtmlAttribute(string tagName)
        {
            Guard.NotNullOrEmpty(tagName, "tagName");
            TagName = tagName;
        }

        public HtmlAttribute(string tagName, bool isSelfClosing)
            : this(tagName)
        {
            IsSelfClosing = isSelfClosing;
        }
    }
}
