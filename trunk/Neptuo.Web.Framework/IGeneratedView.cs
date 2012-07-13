using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Neptuo.Web.Framework
{
    public interface IGeneratedView
    {
        HttpRequest Request { get; set; }

        HttpResponse Response { get; set; }

        IViewPage ViewPage { get; set; }

        void CreateControls();

        void Init();

        void Render(HtmlTextWriter writer);
    }
}
