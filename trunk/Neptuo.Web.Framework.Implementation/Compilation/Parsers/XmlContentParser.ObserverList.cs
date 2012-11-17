using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Neptuo.Web.Framework.Compilation.Parsers
{
    partial class XmlContentParser
    {
        public class ObserverList : List<ParsedObserver> 
        {
            Dictionary<Type, ParsedObserver> byType = new Dictionary<Type, ParsedObserver>();

            public new void Add(ParsedObserver parsedObserver)
            {
                byType[parsedObserver.Type] = parsedObserver;
                base.Add(parsedObserver);
            }

            public ParsedObserver this[Type type]
            {
                get
                {
                    if (byType.ContainsKey(type))
                        return byType[type];

                    return null;
                }
            }

            public bool ContainsKey(Type type)
            {
                return byType.ContainsKey(type);
            }
        }

        public class ParsedObserver
        {
            public Type Type { get; set; }
            public List<XmlAttribute> Attributes { get; set; }
            public ObserverLivecycle Livecycle { get; set; }

            public ParsedObserver(Type type, ObserverLivecycle livecycle, params XmlAttribute[] attributes)
            {
                Type = type;
                Livecycle = livecycle;
                Attributes = new List<XmlAttribute>(attributes);
            }
        }
    }
}
