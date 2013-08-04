using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class DefaultControlBuilderFactory : IComponentBuilderFactory
    {
        protected Type ControlType { get; private set; }

        public DefaultControlBuilderFactory(Type controlType)
        {
            ControlType = controlType;
        }

        public IComponentBuilder CreateBuilder(string prefix, string tagName)
        {
            return new DefaultControlBuilder(ControlType);
        }
    }
}
