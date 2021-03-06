﻿using Neptuo.Activators;
using Neptuo.Compilers.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.ViewActivators
{
    /// <summary>
    /// Implementation of <see cref="IViewActivatorServiceContext"/>
    /// </summary>
    public class DefaultViewActivatorServiceContext : IViewActivatorServiceContext
    {
        public IDependencyProvider DependencyProvider { get; private set; }
        public ICollection<IErrorInfo> Errors { get; private set; }

        public DefaultViewActivatorServiceContext(IDependencyProvider dependencyProvider, ICollection<IErrorInfo> errors = null)
        {
            Ensure.NotNull(dependencyProvider, "dependencyProvider");
            DependencyProvider = dependencyProvider;
            Errors = errors ?? new List<IErrorInfo>();
        }

        public IViewActivatorContext CreateVisitorContext(IViewActivatorService service)
        {
            Ensure.NotNull(service, "service");
            return new DefaultViewActivatorContext(service, this);
        }
    }
}
