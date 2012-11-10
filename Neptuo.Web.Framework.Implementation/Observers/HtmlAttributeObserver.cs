using Neptuo.Web.Framework.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Observers
{
    [Observer(ObserverLivecycle.PerControl)]
    public class HtmlAttributeObserver : IObserver, IAttributeCollection
    {
        public AttributeCollection Attributes { get; protected set; }

        public HtmlAttributeObserver()
        {
            Attributes = new AttributeCollection();
        }

        public void OnInit(ObserverEventArgs e)
        {
            IAttributeCollection control = e.Target as IAttributeCollection;
            if (control != null)
            {
                foreach (KeyValuePair<string, string> attribute in Attributes)
                    control.SetAttribute(attribute.Key, attribute.Value);
            }
        }

        public void Render(ObserverEventArgs e, HtmlTextWriter writer)
        { }

        public void SetAttribute(string name, string value)
        {
            Attributes[name] = value;
        }
    }
}
