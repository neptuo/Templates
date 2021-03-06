﻿using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Service for generating code.
    /// </summary>
    public interface ICodeGeneratorService
    {
        /// <summary>
        /// Registers <paramref name="generator"/> with <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name of generator.</param>
        /// <param name="generator">Code generator.</param>
        ICodeGeneratorService AddGenerator(string name, ICodeGenerator generator);

        /// <summary>
        /// Generates code using code generator registered with <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name of generator.</param>
        /// <param name="codeObject">Root object in AST.</param>
        /// <param name="context">Context.</param>
        bool GeneratedCode(string name, ICodeObject codeObject, ICodeGeneratorServiceContext context);
    }
}
