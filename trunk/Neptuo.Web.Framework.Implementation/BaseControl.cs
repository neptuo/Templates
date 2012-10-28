using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Neptuo.Web.Framework.Annotations;
using System.ComponentModel;
using Neptuo.Web.Framework.Utils;

namespace Neptuo.Web.Framework
{
    /// <summary>
    /// Controls extends this class support tag name specified in <see cref="ControlAttribute"/>.
    /// </summary>
    public abstract class BaseControl : IControl
    {
        public Dictionary<string, string> Attributes { get; protected set; }

        protected virtual string TagName
        {
            get
            {
                HtmlAttribute attr = HtmlAttribute.GetAttribute(GetType());
                if (attr != null)
                    return attr.TagName;

                return null;
            }
        }

        protected virtual bool IsSelfClosing
        {
            get
            {
                HtmlAttribute attr = HtmlAttribute.GetAttribute(GetType());
                if (attr != null)
                    return attr.IsSelfClosing;

                return true;
            }
        }

        protected string DefaultPropertyName
        {
            get
            {
                DefaultPropertyAttribute attr = ReflectionHelper.GetAttribute<DefaultPropertyAttribute>(GetType());
                if (attr != null)
                    return attr.Name;

                return null;
            }
        }

        public BaseControl()
        {
            Attributes = new Dictionary<string, string>();
        }

        public virtual void OnInit() { }

        public virtual void Render(HtmlTextWriter writer)
        {
            ControlAttribute attr = ControlAttribute.GetAttribute(GetType());
            if (!String.IsNullOrEmpty(TagName))
            {
                StringBuilder result = new StringBuilder();
                result.AppendFormat("<{0}", TagName);

                foreach (KeyValuePair<string, string> attribute in Attributes)
                {
                    result.AppendFormat(" {0}=\"{1}\"", attribute.Key, attribute.Value);
                }

                if (IsSelfClosing)
                {
                    result.Append(" />");
                    writer.Write(result);
                }
                else
                {
                    result.Append(">");
                    writer.Write(result);

                    RenderBody(writer);

                    result = new StringBuilder();
                    result.AppendFormat("</{0}>", TagName);
                    writer.Write(result);
                }
            }
            else
            {
                RenderBody(writer);
            }
        }

        protected virtual void RenderBody(HtmlTextWriter writer) { }
    }
}
