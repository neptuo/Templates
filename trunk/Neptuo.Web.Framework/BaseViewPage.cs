using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    public class BaseViewPage : IViewPage
    {
        public List<object> Content { get; set; }

        public BaseViewPage()
        {
            Content = new List<object>();
        }

        public void OnInit()
        {
            foreach (object item in Content)
            {
                IControl control = item as IControl;
                if (control != null)
                    control.OnInit();
            }
        }

        public void Render(HtmlTextWriter writer)
        {
            foreach (object item in Content)
            {
                IControl control = item as IControl;
                if (control != null)
                    control.Render(writer);
            }
        }
    }
}
