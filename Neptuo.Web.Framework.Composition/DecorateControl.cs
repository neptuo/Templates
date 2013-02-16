using Neptuo.Web.Framework.Compilation;
using Neptuo.Web.Framework.Composition.Data;
using Neptuo.Web.Framework.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Composition
{
    [DefaultProperty("Content")]
    public class DecorateControl : IControl, IDisposable
    {
        private IDependencyProvider provider;
        private IComponentManager componentManager;

        private IGeneratedView view;

        //[TypeConverter(typeof(TemplateTypeConverter))]
        public Template Template { get; set; }

        public string Path { get; set; }
        public ICollection<ContentControl> Content { get; set; }

        public DecorateControl(IDependencyProvider provider, IComponentManager componentManager)
        {
            this.provider = provider;
            this.componentManager = componentManager;
        }

        public void OnInit()
        {
            if (Template != null)
                componentManager.Init(Template);

            ContentStorage storage = new ContentStorage();
            if (Content != null)
            {
                foreach (ContentControl content in Content)
                {
                    if (String.IsNullOrEmpty(content.Name))
                        content.Name = String.Empty;

                    componentManager.Init(content);
                    storage.Add(content.Name, content);
                }
            }


            //\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
            //TODO: Je potřeba do svých potomků dostat ContentStorage!\\
            //\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\






            //IViewService viewService = provider.Resolve<IViewService>();
            ////view = viewService.ProcessContent(Template, new DefaultViewServiceContext(provider));
            ////view = viewService.Process(Path, new DefaultViewServiceContext(provider));
            //if (view != null)
            //{
            //    IDependencyContainer childContainer = provider.CreateChildContainer();
            //    childContainer.RegisterInstance<ContentStorage>(storage);

            //    view.Setup(new BaseViewPage(componentManager), componentManager, childContainer);
            //    view.CreateControls();
            //    view.Init();
            //}
        }

        public void Render(HtmlTextWriter writer)
        {
            if (Template != null)
            {
                foreach (object item in Template.Content)
                    componentManager.Render(item, writer);
            }
                
            //if (view != null)
            //    view.Render(writer);
        }

        public void Dispose()
        {
            //if (view != null)
            //    view.Dispose();
        }
    }
}
