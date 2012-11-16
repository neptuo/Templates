using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Configuration
{
    public class ObserversCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ObserverElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((ObserverElement)element).TypeName;
        }

        public ObserverElement this[int index]
        {
            get { return (ObserverElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }

        new public ObserverElement this[string Name]
        {
            get { return (ObserverElement)BaseGet(Name); }
        }

        public int IndexOf(ObserverElement element)
        {
            return BaseIndexOf(element);
        }

        public void Add(ObserverElement element)
        {
            BaseAdd(element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(ObserverElement element)
        {
            if (BaseIndexOf(element) >= 0)
                BaseRemove(element.TypeName);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }
}
