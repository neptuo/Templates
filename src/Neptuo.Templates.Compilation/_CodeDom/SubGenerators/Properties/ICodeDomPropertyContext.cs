﻿using Neptuo.Collections.Specialized;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Context for <see cref="ICodeDomPropertyGenerator"/>.
    /// </summary>
    public interface ICodeDomPropertyContext : ICodeDomContext
    {
        /// <summary>
        /// Storage for custom values.
        /// </summary>
        IReadOnlyKeyValueCollection CustomValues { get; }

        /// <summary>
        /// Object where property generator should set value.
        /// </summary>
        CodeExpression PropertyTarget { get; }
    }
}
