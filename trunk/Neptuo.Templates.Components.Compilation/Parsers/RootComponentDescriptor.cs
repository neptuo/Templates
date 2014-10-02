using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class RootComponentDescriptor : IComponentDescriptor
    {
        IPropertyInfo defaultProperty;
        List<IPropertyInfo> properties;

        public RootComponentDescriptor()
        {
            defaultProperty = new TypePropertyInfo(typeof(GeneratedView).GetProperty(TypeHelper.PropertyName<GeneratedView, ICollection<object>>(v => v.Content)));
            properties = new List<IPropertyInfo>() 
            { 
                defaultProperty 
            };
        }

        public IEnumerable<IPropertyInfo> GetProperties()
        {
            return properties;
        }

        public IPropertyInfo GetDefaultProperty()
        {
            return defaultProperty;
        }
    }
}
