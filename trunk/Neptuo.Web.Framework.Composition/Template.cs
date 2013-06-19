using Neptuo.Web.Framework.Compilation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Composition
{
    [DefaultProperty("Content")]
    //[TypeConverter(typeof(TemplateTypeConverter))]
    public class Template
    {
        private IGeneratedView view;

        public ICollection<object> Content
        {
            //get { return view.Content; }
            get;
            set;
        }

        public virtual void Init(IComponentManager componentManager)
        {
            foreach (object content in Content)
                componentManager.Init(content);
        }

        public virtual void Render(IComponentManager componentManager, HtmlTextWriter writer)
        {
            foreach (object content in Content)
                componentManager.Render(content, writer);
        }

        //public Template(IDependencyProvider provider, IViewService viewService, string content)
        //{
        //    view = viewService.ProcessContent(content, new DefaultViewServiceContext(provider));
        //}
    }

    public class FileTemplate : Template
    {
        private IGeneratedView view;
        private IDependencyProvider dependencyProvider;

        public FileTemplate(IGeneratedView view, IDependencyProvider dependencyProvider)
        {
            this.view = view;
            this.dependencyProvider = dependencyProvider;
        }

        public override void Init(IComponentManager componentManager)
        {
            view.Setup(new BaseViewPage(componentManager), componentManager, dependencyProvider);
            view.CreateControls();
            view.Init();

            Content = view.Content;
        }
    }
}
