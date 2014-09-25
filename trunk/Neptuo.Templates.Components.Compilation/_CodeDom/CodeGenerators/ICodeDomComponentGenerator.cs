using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// CodeDom sub generator for code objects.
    /// </summary>
    public interface ICodeDomComponentGenerator
    {
        /// <summary>
        /// Generates code for <paramref name="codeObject"/>.
        /// </summary>
        /// <param name="context">Context for code generation.</param>
        /// <param name="codeObject">Code object to evaluate.</param>
        /// <param name="propertyDescriptor">Parent property descriptor holds <paramref name="codeObject"/>.</param>
        /// <returns>Generated property value assignment code.</returns>
        CodeExpression GenerateCode(CodeObjectExtensionContext context, ICodeObject codeObject, IPropertyDescriptor propertyDescriptor);
    }
}
