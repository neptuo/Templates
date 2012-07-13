using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.IO;
using Neptuo.Web.Html;
using Neptuo.Web.Parser;
using Neptuo.Web.Parser.Html;
using Neptuo.Web.Html.Configuration;
using Neptuo.Web.Html.Compilation.DirectCreation;
using Neptuo.Web.Html.Compilation;
using Neptuo.Web.Mvc.ViewEngine.Utils;

namespace Neptuo.Web.Mvc.ViewEngine
{
    /// <summary>
    /// Html view.
    /// </summary>
    public class HtmlView : System.Web.Mvc.IView
    {
        public string ViewName { get; protected set; }

        public string MasterName { get; protected set; }

        public bool UseCache { get; protected set; }

        public Configuration Configuration { get; protected set; }

        public HtmlView(Configuration configuration, string viewName, string masterName = null, bool? useCache = null)
        {
            Configuration = configuration;
            ViewName = viewName;
            MasterName = masterName;
            UseCache = useCache ?? true;
        }

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            IViewProcessor processor = new DirectViewProcessor(viewContext, Configuration);
            IViewPage view = processor.LoadView(ViewName);

            processor.OnInit();
            processor.OnLoad();
            processor.BeforeRender();

            foreach (object child in view.Childs)
            {
                IControl control = child as IControl;
                if (control != null)
                    control.Render(new HtmlTextWriter(writer));
            }

            processor.BeforeUnLoad();
        }
    }
}
