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

        //public Template(IDependencyProvider provider, IViewService viewService, string content)
        //{
        //    view = viewService.ProcessContent(content, new DefaultViewServiceContext(provider));
        //}
    }
}
