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
    /// Generator for creating instances of types.
    /// </summary>
    public class CodeDomObjectInstanceGenerator
    {
        /// <summary>
        /// Generates expression for creating instance of type <paramref name="type"/>.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="type">Type to create instance of.</param>
        /// <returns>Expression for creating instance of type <paramref name="type"/>.</returns>
        public CodeExpression GenerateCode(ICodeDomContext context, Type type)
        {
            Guard.NotNull(context, "context");
            Guard.NotNull(type, "type");

            if (context.Configuration.IsDirectObjectResolve())
                return context.Registry.WithDependencyGenerator().Generate(context, type);
            else
                return new CodeObjectCreateExpression(type, ResolveConstructorParameters(context, type));
        }

        /// <summary>
        /// Resolves all constructor parameters (using <code>CodeGenerator.GenerateDependency</code>).
        /// </summary>
        /// <param name="context">Current context.</param>
        /// <param name="type">Target control type to resolve.</param>
        /// <returns>Generated code expressions.</returns>
        protected CodeExpression[] ResolveConstructorParameters(ICodeDomContext context, Type type)
        {
            List<CodeExpression> result = new List<CodeExpression>();
            ConstructorInfo ctor = type.GetConstructors().OrderBy(c => c.GetParameters().Count()).FirstOrDefault();
            if (ctor != null)
            {
                foreach (ParameterInfo parameter in ctor.GetParameters())
                {
                    CodeExpression parameterExpression = context.Registry.WithDependencyGenerator().Generate(context, parameter.ParameterType);
                    if (parameterExpression != null)
                        result.Add(parameterExpression);
                    else
                        throw new NotImplementedException("Not supported parameter type!");
                }
            }

            return result.ToArray();
        }
    }
}
