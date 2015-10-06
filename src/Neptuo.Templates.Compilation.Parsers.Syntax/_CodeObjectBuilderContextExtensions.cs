using Neptuo.Templates.Compilation.Parsers.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax
{
    /// <summary>
    /// Common extensions for <see cref="ICodeObjectBuilderContext"/>.
    /// </summary>
    public static class _CodeObjectBuilderContextExtensions
    {
        /// <summary>
        /// Creates <see cref="ICodePropertyBuilderContext"/>.
        /// </summary>
        /// <param name="context">Code object builder context.</param>
        /// <param name="propertyName">Name of the property to set.</param>
        /// <param name="propertyType">Type of property to set.</param>
        /// <returns>Instance of <see cref="ICodePropertyBuilderContext"/>.</returns>
        public static ICodePropertyBuilderContext CreatePropertyContext(this ICodeObjectBuilderContext context, string propertyName, Type propertyType)
        {
            return new DefaultCodePropertyBuilderContext(context.ParserProvider, propertyName, propertyType);
        }

        /// <summary>
        /// Creates <see cref="ICodePropertyBuilderContext"/>.
        /// </summary>
        /// <param name="context">Code object builder context.</param>
        /// <param name="fieldDescriptor">Field descriptor.</param>
        /// <returns>Instance of <see cref="ICodePropertyBuilderContext"/>.</returns>
        public static ICodePropertyBuilderContext CreatePropertyContext(this ICodeObjectBuilderContext context, IFieldDescriptor fieldDescriptor)
        {
            Ensure.NotNull(fieldDescriptor, "fieldDescriptor");
            return CreatePropertyContext(context, fieldDescriptor.Name, fieldDescriptor.FieldType);
        }
    }
}
