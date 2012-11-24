using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Neptuo.Web.Framework.Compilation.Parsers
{
    public interface IXmlObserverBuilder
    {
        void Parse(IXmlBuilderContext context, IComponentCodeObject codeObject, Type observerType, IEnumerable<XmlAttribute> attributes, ObserverLivecycle livecycle);
    }
}
