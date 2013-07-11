using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Compilation.Parsers;
using Neptuo.Web.Framework.Observers;
using Neptuo.Web.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Neptuo.Web.Framework.Composition
{
    [Builder(typeof(ContentXmlObserverBuilder))]
    public class ContentObserver : IObserver
    {
        public void OnInit(ObserverEventArgs e)
        { }

        public void Render(ObserverEventArgs e, HtmlTextWriter writer)
        { }
    }

    public class ContentXmlObserverBuilder : IXmlObserverBuilder
    {
        public void Parse(IXmlBuilderContext context, IComponentCodeObject codeObject, Type observerType, IEnumerable<XmlAttribute> attributes, ObserverLivecycle livecycle)
        {
            ListAddPropertyDescriptor parent = (ListAddPropertyDescriptor)context.Parent;

            parent.Values.Remove(codeObject);

            ControlCodeObject controlCodeObject = new ControlCodeObject(typeof(ContentControl));
            controlCodeObject.Properties.Add(new SetPropertyDescriptor(typeof(ContentControl).GetProperty(TypeHelper.PropertyName<ContentControl>(c => c.Name)), new PlainValueCodeObject(attributes.First().Value)));
            controlCodeObject.Properties.Add(new ListAddPropertyDescriptor(typeof(ContentControl).GetProperty(TypeHelper.PropertyName<ContentControl>(c => c.Content)), codeObject));
            parent.Values.Add(controlCodeObject);
        }
    }

}
