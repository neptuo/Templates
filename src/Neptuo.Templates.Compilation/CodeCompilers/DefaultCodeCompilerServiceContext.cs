﻿using Neptuo.Activators;
using Neptuo.Compilers.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeCompilers
{
    /// <summary>
    /// Implementation of <see cref="ICodeCompilerServiceContext"/>.
    /// </summary>
    public class DefaultCodeCompilerServiceContext : ICodeCompilerServiceContext
    {
        public IDependencyProvider DependencyProvider { get; private set; }
        public ICollection<IErrorInfo> Errors { get; private set; }

        public DefaultCodeCompilerServiceContext(IDependencyProvider dependencyProvider, ICollection<IErrorInfo> errors = null)
        {
            Ensure.NotNull(dependencyProvider, "dependencyProvider");
            DependencyProvider = dependencyProvider;
            Errors = errors ?? new List<IErrorInfo>();
        }

        public ICodeCompilerContext CreateCompilerContext(ICodeCompilerService service)
        {
            Ensure.NotNull(service, "service");
            return new DefaultCodeCompilerContext(service, this);
        }
    }
}
