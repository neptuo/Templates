﻿using Neptuo.Activators;
using Neptuo.Compilers.Errors;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Implementation of <see cref="IParserServiceContext"/>.
    /// </summary>
    public class DefaultParserServiceContext : IParserServiceContext
    {
        public IDependencyProvider DependencyProvider { get; private set; }
        public ICollection<IErrorInfo> Errors { get; private set; }

        public DefaultParserServiceContext(IDependencyProvider dependencyProvider, ICollection<IErrorInfo> errors = null)
        {
            Ensure.NotNull(dependencyProvider, "dependencyProvider");
            DependencyProvider = dependencyProvider;
            Errors = errors ?? new List<IErrorInfo>();
        }

        public IContentParserContext CreateContentContext(string name, IParserService service)
        {
            Ensure.NotNull(service, "service");
            return new DefaultParserContext(name, service, this);
        }

        public IValueParserContext CreateValueContext(string name, IParserService service)
        {
            Ensure.NotNull(service, "service");
            return new DefaultParserContext(name, service, this);
        }
    }
}
