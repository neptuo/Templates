using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators
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
        void AddGenerator(string name, ICodeGenerator generator);

        /// <summary>
        /// Generates code using code generator registered with <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name of generator.</param>
        /// <param name="propertyDescriptor">Root property in AST.</param>
        /// <param name="context">Context.</param>
        bool GeneratedCode(string name, IPropertyDescriptor propertyDescriptor, ICodeGeneratorServiceContext context);
    }
}
