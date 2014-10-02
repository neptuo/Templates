using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class DefaultTypeComponentBuilderFactory : IContentBuilderFactory
    {
        protected Type ControlType { get; private set; }

        public DefaultTypeComponentBuilderFactory(Type controlType)
        {
            ControlType = controlType;
        }

        public IContentBuilder CreateBuilder(string prefix, string tagName)
        {
            return new DefaultTypeComponentBuilder(ControlType);
        }
    }
}
