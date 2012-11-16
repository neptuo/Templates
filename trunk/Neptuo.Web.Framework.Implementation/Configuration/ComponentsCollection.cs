using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Configuration
{
    public class ComponentsCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ComponentElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((ComponentElement)element).Namespace;
        }

        public ComponentElement this[int index]
        {
            get { return (ComponentElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }

        new public ComponentElement this[string Name]
        {
            get { return (ComponentElement)BaseGet(Name); }
        }

        public int IndexOf(ComponentElement element)
        {
            return BaseIndexOf(element);
        }

        public void Add(ComponentElement element)
        {
            BaseAdd(element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(ComponentElement element)
        {
            if (BaseIndexOf(element) >= 0)
                BaseRemove(element.Namespace);
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
