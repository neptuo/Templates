using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Specialized list of observers.
    /// </summary>
    public class ObserverCollection : List<ObserverCollection.ItemValue>
    {
        private readonly Dictionary<IObserverRegistration, ItemValue> byBuilder = new Dictionary<IObserverRegistration, ItemValue>();

        public void Add(IObserverRegistration observer, params IXmlAttribute[] attributes)
        {
            Add(new ItemValue(observer, attributes));
        }

        public new void Add(ItemValue parsedObserver)
        {
            byBuilder[parsedObserver.Observer] = parsedObserver;
            base.Add(parsedObserver);
        }

        public ItemValue this[IObserverRegistration observer]
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

        public class ItemValue
        {
            public IObserverRegistration Observer { get; private set; }
            public List<IXmlAttribute> Attributes { get; private set; }

            public ItemValue(IObserverRegistration observer, params IXmlAttribute[] attributes)
            {
                Observer = observer;
                Attributes = new List<IXmlAttribute>(attributes);
            }
        }
    }
}
