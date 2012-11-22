using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Configuration
{
    public class PathCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new PathElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((PathElement)element).Path;
        }

        public PathElement this[int index]
        {
            get { return (PathElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }

        new public PathElement this[string Name]
        {
            get { return (PathElement)BaseGet(Name); }
        }

        public int IndexOf(PathElement element)
        {
            return BaseIndexOf(element);
        }

        public void Add(PathElement element)
        {
            BaseAdd(element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(PathElement element)
        {
            if (BaseIndexOf(element) >= 0)
                BaseRemove(element.Path);
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
