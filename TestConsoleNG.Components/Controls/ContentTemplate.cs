using Neptuo.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleNG.Controls
{
    public class ContentTemplate : ITemplate
    {
        protected IComponentManager ComponentManager { get; private set; }
        public Action<ContentTemplateContent> BindMethod { get; set; }

        public ContentTemplate(IComponentManager componentManager)
        {
            ComponentManager = componentManager;
        }

        public ITemplateContent CreateInstance()
        {
            ContentTemplateContent templateContent = new ContentTemplateContent(ComponentManager);
            ComponentManager.AddComponent(templateContent, BindMethod);
            return templateContent;
        }

        public void Dispose()
        { }
    }

    public class ContentTemplateContent : ITemplateContent
    {
        protected IComponentManager ComponentManager { get; private set; }
        public ICollection<object> Content { get; set; }

        public ContentTemplateContent(IComponentManager componentManager)
        {
            ComponentManager = componentManager;
        }

        public void OnInit()
        {
            if (Content != null)
            {
                foreach (object item in Content)
                    ComponentManager.Init(item);
            }
        }

        public void Render(IHtmlWriter writer)
        {
            if (Content != null)
            {
                foreach (object item in Content)
                    ComponentManager.Render(item, writer);
            }
        }

        public void Dispose()
        {
            if (Content != null)
            {
                foreach (object item in Content)
                    ComponentManager.Dispose(item);
            }
        }
    }

}
