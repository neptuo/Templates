using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Neptuo.Templates.Compilation.Parsers
{
    partial class XmlContentParser
    {
        public class ObserverList : List<ParsedObserver> 
        {
            Dictionary<IObserverBuilder, ParsedObserver> byBuilder = new Dictionary<IObserverBuilder, ParsedObserver>();

            public new void Add(ParsedObserver parsedObserver)
            {
                byBuilder[parsedObserver.ObserverBuilder] = parsedObserver;
                base.Add(parsedObserver);
            }

            public ParsedObserver this[IObserverBuilder type]
            {
                get
                {
                    if (byBuilder.ContainsKey(type))
                        return byBuilder[type];

                    return null;
                }
            }

            public bool ContainsKey(IObserverBuilder type)
            {
                return byBuilder.ContainsKey(type);
            }
        }

        public class ParsedObserver
        {
            public IObserverBuilder ObserverBuilder { get; set; }
            public List<XmlAttribute> Attributes { get; set; }

            public ParsedObserver(IObserverBuilder observerBuilder, params XmlAttribute[] attributes)
            {
                ObserverBuilder = observerBuilder;
                Attributes = new List<XmlAttribute>(attributes);
            }
        }
    }
}
