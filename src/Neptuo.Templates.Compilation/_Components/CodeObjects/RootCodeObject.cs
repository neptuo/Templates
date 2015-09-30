using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Describes root of AST.
    /// </summary>
    public class RootCodeObject : IFieldCollectionCodeObject
    {
        protected List<ICodeProperty> Properties { get; set; }

        public RootCodeObject()
        {
            Properties = new List<ICodeProperty>();
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
