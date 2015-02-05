using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Property value generator based on used attributes.
    /// </summary>
    public class CodeDomPropertyDefaultValueFeature
    {
        /// <summary>
        /// If <c>true</c>, <see cref="CodeDomPropertyDefaultValueFeature.TryGenerate"/> tries to use attribute generators.
        /// This value overrides <see cref="IsAttributeDefaultEnabled"/>.
        /// </summary>
        public bool? IsAttributeDefaultEnabled { get; set; }

        /// <summary>
        /// If <c>true</c>, <see cref="CodeDomPropertyDefaultValueFeature.TryGenerate"/> tries to use unbound property typy generators.
        /// This value overrides <see cref="IsPropertyTypeDefaultEnabled"/>.
        /// </summary>
        public bool? IsPropertyTypeDefaultEnabled { get; set; }

        /// <summary>
        /// Tries to generate <paramref name="expression"/> based on attributes used on <paramref name="propertyInfo"/>.
        /// If processing has no error, returns <c>true</c>; otherwise returns <c>false</c>.
        /// <paramref name="expression"/> is set, if any attribute provided expression.
        /// So, method can return <c>true</c> and <paramref name="expression"/> can be <c>null</c> (no error, no expression).
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="propertyInfo">Property info to process.</param>
        /// <param name="expression">Output generated expression.</param>
        /// <returns><c>true</c> if processing has no error; otherwise <c>false</c>.</returns>
        public bool TryGenerate(ICodeDomContext context, PropertyInfo propertyInfo, out CodeExpression expression)
        {
            expression = null;

            // If configuration enables it...
            if (IsAttributeDefaultEnabled ?? context.Configuration.IsAttributeDefaultEnabled())
            {
                // ... try process attributes.
                foreach (Attribute attribute in propertyInfo.GetCustomAttributes())
                {
                    ICodeDomAttributeResult attributeResult = context.Registry.WithAttributeGenerator().Generate(context, attribute);
                    if (attributeResult == null)
                        return false;

                    if (attributeResult.HasExpression())
                    {
                        expression = attributeResult.Expression;
                        return true;
                    }
                }
            }

            // If configuration enables it...
            if (IsPropertyTypeDefaultEnabled ?? context.Configuration.IsPropertyTypeDefaultEnabled())
            {
                // ... try process unbound property by property type.
                ICodeDomPropertyTypeResult propertyResult = context.Registry.WithUnboundPropertyType().Generate(context, propertyInfo);
                if (propertyResult == null)
                    return false;

                if (propertyResult.HasExpression())
                {
                    expression = propertyResult.Expression;
                    return true;
                }
            }

            // No error, no expression.
            return true;
        }
    }
}
