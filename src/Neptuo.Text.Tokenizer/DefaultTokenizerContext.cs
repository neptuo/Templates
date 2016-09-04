﻿using Neptuo.Activators;
using Neptuo.Compilers.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Text
{
    /// <summary>
    /// Default implementation of <see cref="ITokenizerContext"/>.
    /// </summary>
    public class DefaultTokenizerContext : ITokenizerContext
    {
        public IDependencyProvider DependencyProvider { get; private set; }
        public ICollection<IErrorInfo> Errors { get; private set; }

        public DefaultTokenizerContext(IDependencyProvider dependencyProvider, ICollection<IErrorInfo> errors = null)
        {
            Ensure.NotNull(dependencyProvider, "dependencyProvider");
            DependencyProvider = dependencyProvider;
            Errors = errors ?? new List<IErrorInfo>();
        }
    }
}
