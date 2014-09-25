using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    partial class XmlContentParser
    {
        /// <summary>
        /// Specialized list of observers.
        /// </summary>
        public class ObserverList : List<ParsedObserver> 
        {
            Dictionary<IObserverRegistration, ParsedObserver> byBuilder = new Dictionary<IObserverRegistration, ParsedObserver>();

            public new void Add(ParsedObserver parsedObserver)
            {
                byBuilder[parsedObserver.Observer] = parsedObserver;
                base.Add(parsedObserver);
            }

            public ParsedObserver this[IObserverRegistration observer]
            {
                get
                {
                    if (byBuilder.ContainsKey(observer))
                        return byBuilder[observer];

                    return null;
                }
            }

            public bool ContainsKey(IObserverRegistration observer)
            {
                return byBuilder.ContainsKey(observer);
            }
        }

        public class ParsedObserver
        {
            public IObserverRegistration Observer { get; set; }
            public List<IXmlAttribute> Attributes { get; set; }

            public ParsedObserver(IObserverRegistration observer, params IXmlAttribute[] attributes)
            {
                Observer = observer;
                Attributes = new List<IXmlAttribute>(attributes);
            }
        }
    }
}
