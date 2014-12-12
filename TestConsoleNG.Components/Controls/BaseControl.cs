using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using Neptuo.Templates;
using Neptuo.Templates.Controls;
using Neptuo.Reflection;

namespace TestConsoleNG.Controls
{
    /// <summary>
    /// Controls extends this class support tag name specified in <see cref="ComponentAttribute"/>.
    /// </summary>
    public abstract class BaseControl : IControl, IHtmlAttributeCollectionAware
    {
        private string tagName;
        private bool? isSelfClosing;
        private string defaultPropertyName;

        public HtmlAttributeCollection HtmlAttributes { get; protected set; }

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
            HtmlAttributes = new HtmlAttributeCollection();
        }

        public virtual void OnInit() { }

        public virtual void Render(IHtmlWriter writer)
        {
            if (!String.IsNullOrEmpty(TagName))
            {
                writer.Tag(TagName);

                foreach (KeyValuePair<string, string> attribute in HtmlAttributes)
                    writer.Attribute(attribute.Key, attribute.Value);

                
                if (IsSelfClosing)
                {
                    writer.CloseTag();
                }
                else
                {
                    RenderBody(writer);
                    writer.CloseFullTag();
                }
            }
            else
            {
                RenderBody(writer);
            }
        }

        protected virtual void RenderBody(IHtmlWriter writer) { }

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
            HtmlAttributes[name] = value;
        }
    }
}
