using Neptuo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.UI
{
    public class ContentTemplate : ITemplate
    {
        public Action<ContentTemplateContent> BindMethod { get; set; }

        public ITemplateContent CreateInstance(IComponentManager componentManager)
        {
            ContentTemplateContent templateContent = new ContentTemplateContent();
            componentManager.AddComponent(templateContent, BindMethod);
            return templateContent;
        }

        public void Dispose()
        { }
    }

    public class ContentTemplateContent : ITemplateContent
    {
        protected IComponentManager ComponentManager { get; private set; }
        public ICollection<object> Content { get; set; }

        public void OnInit(IComponentManager componentManager)
        {
            Guard.NotNull(componentManager, "componentManager");
            ComponentManager = componentManager;

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
