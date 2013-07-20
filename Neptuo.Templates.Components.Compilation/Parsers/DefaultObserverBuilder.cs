using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class DefaultObserverBuilder : BaseObserverBuilder
    {
        protected override Type GetObserverType(IEnumerable<XmlAttribute> attributes)
        {
            throw new NotImplementedException();
        }
    }
}
