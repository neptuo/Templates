using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Configuration
{
    public class ComponentsSection : ConfigurationSection
    {
        [ConfigurationProperty("namespaces", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ComponentsCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public ComponentsCollection Namespaces
        {
            get { return (ComponentsCollection)this["namespaces"]; }
        }

        [ConfigurationProperty("observers", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ObserversCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public ObserversCollection Observers
        {
            get { return (ObserversCollection)this["observers"]; }
        }
    }
}
