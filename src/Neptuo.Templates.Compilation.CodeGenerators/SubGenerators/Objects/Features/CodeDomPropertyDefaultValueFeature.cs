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
            foreach (Attribute attribute in propertyInfo.GetCustomAttributes())
            {
                ICodeDomAttributeResult result = context.Registry.WithAttributeGenerator().Generate(context, attribute);
                if (result == null)
                {
                    expression = null;
                    return false;
                }

                if (result.HasExpression())
                {
                    expression = result.Expression;
                    return true;
                }
            }

            expression = null;
            return true;
        }
    }
}
