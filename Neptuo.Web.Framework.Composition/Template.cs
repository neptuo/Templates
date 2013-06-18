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

        public void Init(IComponentManager componentManager)
        {
            foreach (object content in Content)
                componentManager.Init(content);
        }

        public void Render(IComponentManager componentManager, HtmlTextWriter writer)
        {
            foreach (object content in Content)
                componentManager.Render(content, writer);
        }

        //public Template(IDependencyProvider provider, IViewService viewService, string content)
        //{
        //    view = viewService.ProcessContent(content, new DefaultViewServiceContext(provider));
        //}
    }
}
