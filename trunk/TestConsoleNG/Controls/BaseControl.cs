using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using Neptuo.Templates;
using Neptuo.Templates.Controls;

namespace TestConsoleNG.Controls
{
    /// <summary>
    /// Controls extends this class support tag name specified in <see cref="ComponentAttribute"/>.
    /// </summary>
    public abstract class BaseControl : IControl, IAttributeCollection
    {
        private string tagName;
        private bool? isSelfClosing;
        private string defaultPropertyName;

        public HtmlAttributeCollection Attributes { get; protected set; }

        protected IComponentManager ComponentManager { get; private set; }
        protected virtual string TagName
        {
            get
            {
                if (tagName == null)
                {

                    HtmlAttribute attr = ReflectionHelper.GetAttribute<HtmlAttribute>(GetType());
                    if (attr != null)
                        tagName = attr.TagName;
                }
                return tagName;
            }
            set { tagName = value; }
        }

        protected virtual bool IsSelfClosing
        {
            get
            {
                if (isSelfClosing == null)
                {
                    HtmlAttribute attr = ReflectionHelper.GetAttribute<HtmlAttribute>(GetType());
                    if (attr != null)
                        isSelfClosing = attr.IsSelfClosing;

                }
                return isSelfClosing ?? true;
            }
            set { isSelfClosing = value; }
        }

        protected string DefaultPropertyName
        {
            get
            {
                if (defaultPropertyName == null)
                {
                    DefaultPropertyAttribute attr = ReflectionHelper.GetAttribute<DefaultPropertyAttribute>(GetType());
                    if (attr != null)
                        defaultPropertyName = attr.Name;
                }
                return defaultPropertyName;
            }
            set { defaultPropertyName = value; }
        }

        public BaseControl(IComponentManager componentManager)
        {
            ComponentManager = componentManager;
            Attributes = new HtmlAttributeCollection();
        }

        public virtual void OnInit() { }

        public virtual void Render(HtmlTextWriter writer)
        {
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

        protected void Init(object component)
        {
            ComponentManager.Init(component);
        }

        protected void Init<T>(IEnumerable<T> compoments)
        {
            if (compoments != null)
            {
                foreach (T component in compoments)
                    Init(component);
            }
        }

        public void SetAttribute(string name, string value)
        {
            Attributes[name] = value;
        }
    }
}
