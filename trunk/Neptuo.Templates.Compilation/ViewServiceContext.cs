using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Actual implementaion of <see cref="IViewServiceContext"/>.
    /// </summary>
    public class ViewServiceContext : IViewServiceContext
    {
        public IDependencyProvider DependencyProvider { get; set; }

        public ViewServiceContext(IDependencyProvider dependencyProvider)
        {
            DependencyProvider = dependencyProvider;
        }
    }
}
