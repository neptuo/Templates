using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators.CodeObjects
{
    public class XCodeDomObjectInstanceGenerator
    {
        public CodeExpression GenerateCode(XCodeDomGenerator.Context context, Type type, CodeStatementCollection statements)
        {
            if (context.IsDirectObjectResolve)
            {
                return context.CodeGenerator.GenerateDependency(context, type);
            }
            else
            {
                return new CodeObjectCreateExpression(
                    type,
                    ResolveConstructorParameters(context, type)
                );
            }
        }

        /// <summary>
        /// Resolves all constructor parameters (using <code>CodeGenerator.GenerateDependency</code>).
        /// </summary>
        /// <param name="context">Current context.</param>
        /// <param name="type">Target control type to resolve.</param>
        /// <returns>Generated code expressions.</returns>
        protected CodeExpression[] ResolveConstructorParameters(XCodeDomGenerator.Context context, Type type)
        {
            List<CodeExpression> result = new List<CodeExpression>();
            ConstructorInfo ctor = type.GetConstructors().OrderBy(c => c.GetParameters().Count()).FirstOrDefault();
            if (ctor != null)
            {
                foreach (ParameterInfo parameter in ctor.GetParameters())
                {
                    CodeExpression parameterExpression = context.CodeGenerator.GenerateDependency(context, parameter.ParameterType);
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
