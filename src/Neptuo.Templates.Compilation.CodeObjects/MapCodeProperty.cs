using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Code property for mapping keys to values.
    /// Two possible usages:
    /// 1) Call method <see cref="MapCodeProperty.SetKey"/> to define key for all next calls to <see cref="MapCodeProperty.SetValue"/>.
    /// 2) Use <see cref="MapCodeProperty.SetKeyValue"/> to "one-time" key-value assigment.
    /// </summary>
    public class MapCodeProperty : ICodeProperty
    {
        /// <summary>
        /// Dictionary of all values.
        /// </summary>
        public IDictionary<ICodeObject, ICodeObject> Values { get; private set; }

        /// <summary>
        /// Current for all calls to <see cref="MapCodeProperty.SetValue"/>.
        /// </summary>
        public ICodeObject CurrentKey { get; private set; }

        public string Name { get; private set; }
        public Type Type { get; private set; }

        public MapCodeProperty(string name, Type type)
        {
            Ensure.NotNull(name, "name");
            Ensure.NotNull(type, "type");
            Name = name;
            Type = type;
            Values = new Dictionary<ICodeObject, ICodeObject>();
        }

        /// <summary>
        /// Assigns <paramref name="value"/> to <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value assigned to <paramref name="key"/>.</param>
        public void SetKeyValue(ICodeObject key, ICodeObject value)
        {
            Ensure.NotNull(key, "key");
            Ensure.NotNull(value, "value");
            Values[key] = value;
        }

        /// <summary>
        /// Sets current key for all calls to <see cref="MapCodeProperty.SetValue"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        public void SetKey(ICodeObject key)
        {
            CurrentKey = key;
        }

        public void SetValue(ICodeObject value)
        {
            SetKeyValue(CurrentKey, value);
        }
    }
}
