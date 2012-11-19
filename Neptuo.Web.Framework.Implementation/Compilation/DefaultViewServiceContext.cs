using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public class DefaultViewServiceContext : IViewServiceContext
    {
        public IDependencyProvider DependencyProvider { get; set; }

        public DefaultViewServiceContext(IDependencyProvider dependencyProvider)
        {
            DependencyProvider = dependencyProvider;
        }
    }
}
