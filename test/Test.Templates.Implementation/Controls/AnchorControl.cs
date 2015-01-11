using Neptuo.Templates;
using Neptuo.Templates.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Templates.Controls
{
    [Html("a")]
    public class AnchorControl : BaseContentControl
    {
        public string Url { get; set; }
        public ICollection<Parameter> Parameters { get; private set; }
        public Security Security { get; set; }

        public AnchorControl()
        {
            Parameters = new List<Parameter>();
        }

        public override void OnInit(IComponentManager componentManager)
        {
            base.OnInit(componentManager);

            Init(Security);
            if (Security != null)
            {
                Parameters.Add(new Parameter
                {
                    Name = "Security",
                    Value = Security.Identifier
                });
            }

            Init(Parameters);
            foreach (Parameter parameter in Parameters)
            {
                if (Url.Contains("?"))
                    Url += "&";
                else
                    Url += "?";

                Url += String.Format("{0}={1}", parameter.Name, parameter.Value);
            }
            HtmlAttributes["href"] = Url;
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
