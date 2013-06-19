using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Controls;
using System.Web;

namespace Neptuo.Web.Framework.Mvc.Controls
{
    [Html("a")]
    public class ActionControl : BaseContentControl
    {
        private IRequestHelper requestHelper;

        public string Controller { get; set; }
        public string Action { get; set; }

        public ActionControl(IRequestHelper requestHelper)
        {
            this.requestHelper = requestHelper;
        }

        public override void OnInit()
        {
            string url = requestHelper.ApplicationPath;
            if (!String.IsNullOrEmpty(Controller))
                url += "/" + Controller;

            if (!String.IsNullOrEmpty(Action))
                url += "/" + Action;

            if (url.StartsWith("//"))
                url = url.Substring(1);

            Attributes["href"] = url;

            if (requestHelper.AppRelativeCurrentExecutionFilePath == "~" + url)
                Attributes["class"] = "active";

            base.OnInit();
        }
    }
}
