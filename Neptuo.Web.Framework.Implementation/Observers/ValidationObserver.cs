using Neptuo.Web.Framework.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Observers
{
    [Observer(ObserverLivecycle.PerControl)]
    public class ValidationObserver : IObserver
    {
        public string Regex { get; set; }
        public string ErrorMessage { get; set; }
        [DefaultValue(-1)]
        public int MaxLength { get; set; }
        [DefaultValue(-1)]
        public int MinLength { get; set; }

        public void OnInit(ObserverEventArgs e)
        {
            BaseControl control = e.Target as BaseControl;
            if (control != null)
            {
                if (!String.IsNullOrEmpty(Regex))
                    control.Attributes["data-val-regex"] = Regex;
                
                if (!String.IsNullOrEmpty(ErrorMessage))
                    control.Attributes["data-val-message"] = ErrorMessage;

                if (MaxLength > 0)
                    control.Attributes["data-val-maxlength"] = MaxLength.ToString();

                if (MinLength > 0)
                    control.Attributes["data-val-minlength"] = MinLength.ToString();
            }
        }

        public void Render(ObserverEventArgs e, HtmlTextWriter writer)
        {
            
        }
    }
}
