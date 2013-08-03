using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Controls
{
    [Html("a")]
    public class AnchorControl : BaseContentControl
    {
        public string Url { get; set; }
        public ICollection<Parameter> Parameters { get; set; }
        public Security Security { get; set; }

        public AnchorControl(IComponentManager componentManager)
            : base(componentManager)
        { }

        public override void OnInit()
        {
            base.OnInit();

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
