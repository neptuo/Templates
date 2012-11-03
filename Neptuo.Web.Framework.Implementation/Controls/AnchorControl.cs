using Neptuo.Web.Framework.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Controls
{
    [Html("a")]
    public class AnchorControl : BaseContentControl
    {
        public string Url { get; set; }
        public ICollection<Parameter> Parameters { get; set; }
        public Security Security { get; set; }

        public override void OnInit()
        {
            base.OnInit();
            ((ComponentManagerCollection<Parameter>)Parameters).Init();


            foreach (Parameter parameter in Parameters)
            {
                if (Url.Contains("?"))
                    Url += "&";
                else
                    Url += "?";

                Url += String.Format("{0}={1}", parameter.Name, parameter.Value);
            }
            Attributes["href"] = Url;
        }
    }

    public class Parameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Security
    {
        public string Identifier { get; set; }
    }
}
