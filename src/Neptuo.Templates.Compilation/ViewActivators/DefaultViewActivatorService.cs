﻿using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.ViewActivators
{
    /// <summary>
    /// Implementation of <see cref="IViewActivatorService"/>.
    /// </summary>
    public class DefaultViewActivatorService : IViewActivatorService
    {
        private readonly Dictionary<string, IViewActivator> activators = new Dictionary<string, IViewActivator>();

        public IViewActivatorService AddActivator(string name, IViewActivator activator)
        {
            Guard.NotNullOrEmpty(name, "name");
            Guard.NotNull(activator, "activator");
            activators[name] = activator;
            return this;
        }

        public object Activate(string name, ISourceContent content, IViewActivatorServiceContext context)
        {
            Guard.NotNullOrEmpty(name, "name");
            Guard.NotNull(content, "content");
            Guard.NotNull(context, "context");
            IViewActivator activator;
            if (activators.TryGetValue(name, out activator))
                return activator.Activate(content, context.CreateVisitorContext(this));

            throw Guard.Exception.ArgumentOutOfRange("name", "Requested an unregistered view activator named '{0}'.", name);
        }
    }
}