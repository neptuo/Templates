using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Configuration
{
    public class ComponentsSection : ConfigurationSection
    {
        [ConfigurationProperty("controls", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ComponentsCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public ComponentsCollection Controls
        {
            get { return (ComponentsCollection)this["controls"]; }
        }

        [ConfigurationProperty("extensions", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ComponentsCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public ComponentsCollection Extensions
        {
            get { return (ComponentsCollection)this["extensions"]; }
        }

        [ConfigurationProperty("observers", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ObserversCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public ObserversCollection Observers
        {
            get { return (ObserversCollection)this["observers"]; }
        }
    }
}
