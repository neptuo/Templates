using Neptuo.Models.Features;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Generator for processing properties set on <see cref="IPropertiesCodeObject"/>.
    /// </summary>
    public class CodeDomAstPropertyFeature
    {
        /// <summary>
        /// For each property on <paramref name="codeObject"/>, generates code statement using property registry in <paramref name="context"/>.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="codeObject">Code object to process.</param>
        /// <param name="variableName">Name of the variable for <paramref name="codeObject"/>.</param>
        /// <returns>Enumeration of statements for properties on <paramref name="codeObject"/>.</returns>
        public IEnumerable<CodeStatement> Generate(ICodeDomContext context, IComponentCodeObject codeObject, string variableName)
        {
            Type componentType = null;
            ITypeCodeObject typeCodeObject;
            if (codeObject.TryWith<ITypeCodeObject>(out typeCodeObject))
                componentType = typeCodeObject.Type;

            List<CodeStatement> statements = new List<CodeStatement>();
            CodeExpression variableExpression = new CodeVariableReferenceExpression(variableName);
            foreach (ICodeProperty codeProperty in codeObject.With<IFieldCollectionCodeObject>().EnumerateProperties())
            {
                ICodeDomPropertyResult result = context.Registry.WithPropertyGenerator().Generate(
                    context.CreatePropertyContext(variableExpression).AddFieldType(componentType),
                    codeProperty
                );

                if (result == null)
                    return null;

                statements.AddRange(result.Statements);
            }

            return statements;
        }
    }
}
