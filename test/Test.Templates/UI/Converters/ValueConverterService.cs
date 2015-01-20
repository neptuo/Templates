using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.UI.Converters
{
    public class ValueConverterService : IValueConverterService
    {
        private Dictionary<string, IValueConverter> storage = new Dictionary<string, IValueConverter>();

        public IValueConverter GetConverter(string key)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "Key can't be null or empty!");

            if (!storage.ContainsKey(key))
                throw new ArgumentOutOfRangeException("key", "There is no converter associated with key {0}!", key);

            return storage[key];
        }

        public IValueConverterService SetConverter(string key, IValueConverter converter)
        {
            storage[key] = converter;
            return this;
        }
    }
}
