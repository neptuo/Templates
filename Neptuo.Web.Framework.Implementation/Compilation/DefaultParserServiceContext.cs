using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public class DefaultParserServiceContext : IParserServiceContext
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public ICodeObject RootObject { get; private set; }

        public DefaultParserServiceContext(IServiceProvider serviceProvider, ICodeObject rootObject)
        {
            ServiceProvider = serviceProvider;
            RootObject = rootObject;
        }
    }
}
