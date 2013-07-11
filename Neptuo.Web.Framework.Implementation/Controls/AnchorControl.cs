using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            Init(Security);
            if (Security != null)
            {
                //Parameters.Add(new Parameter
                //{
                //    Name = "Security",
                //    Value = Security.Identifier
                //});
            }

            Init(Parameters);
            foreach (Parameter parameter in Parameters)
            {
                if (Url.Contains("?"))
                    Url += "&";
                else
                    Url += "?";

                //Url += String.Format("{0}={1}", parameter.Name, parameter.Value);
            }
            Attributes["href"] = Url;
        }
    }

    [DefaultProperty("Values")]
    public class Parameter
    {
        public string Name { get; set; }
        public List<ParameterValue> Values { get; set; }

        public Parameter()
        {
            Values = new List<ParameterValue>();
        }
    }

    public class Security
    {
        public string Identifier { get; set; }
    }

    [DefaultProperty("Value")]
    public class ParameterValue
    {
        public string Value { get; set; }
    }
}
