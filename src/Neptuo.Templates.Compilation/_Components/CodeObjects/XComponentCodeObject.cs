using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Control.
    /// </summary>
    public class XComponentCodeObject : ITypeCodeObject, IObserversCodeObject, IFieldCollectionCodeObject
    {
        public Type Type { get; set; }
        protected List<ICodeProperty> Properties { get; set; }
        public List<ICodeObject> Observers { get; set; }

        public XComponentCodeObject(Type type)
        {
            Type = type;
            Properties = new List<ICodeProperty>();
            Observers = new List<ICodeObject>();
        }

        public void AddProperty(ICodeProperty property)
        {
            Properties.Add(property);
        }

        public bool TryGetProperty(string propertyName, out ICodeProperty property)
        {
            property = Properties.FirstOrDefault(p => p.Name == propertyName);
            return property != null;
        }

        public IEnumerable<ICodeProperty> EnumerateProperties()
        {
            return Properties;
        }
    }
}
