using Neptuo.Models.Features;
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
    public class RootCodeObject : IComponentCodeObject, IFieldCollectionCodeObject, ITypeCodeObject
    {
        protected List<ICodeProperty> Properties { get; set; }

        public Type Type { get; private set; }

        public RootCodeObject(Type type)
        {
            Ensure.NotNull(type, "type");
            Type = type;
            Properties = new List<ICodeProperty>();
        }

        public bool TryWith<TFeature>(out TFeature feature)
        {
            Type featureType = typeof(TFeature);
            if (featureType == typeof(IFieldCollectionCodeObject) || featureType == typeof(ITypeCodeObject))
            {
                feature = (TFeature)(object)this;
                return true;
            }

            feature = default(TFeature);
            return false;
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
