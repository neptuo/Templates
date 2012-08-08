using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Neptuo.Web.Framework.Compilation
{
    public interface IXmlObserverBuilder
    {
        void GenerateObserver(Type observerType, XmlAttribute source, XmlBuilderContext context);
    }
}
