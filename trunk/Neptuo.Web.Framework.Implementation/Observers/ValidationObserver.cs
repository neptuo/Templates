using Neptuo.Web.Framework.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Observers
{
    [Observer(ObserverLivecycle.PerControl)]
    public class ValidationObserver : IObserver
    {
        public string Regex { get; set; }
        public string ErrorMessage { get; set; }
        public int MaxLength { get; set; }
        public int MinLength { get; set; }

        public void OnInit(ObserverEventArgs e)
        {
            
        }

        public void Render(ObserverEventArgs e, HtmlTextWriter writer)
        {
            
        }
    }
}
