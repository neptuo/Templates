using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public abstract class CodeDomPropertyGeneratorBase<T> : ICodeDomPropertyGenerator
        where T : ICodeProperty
    {
        public CodeStatement Generate(ICodeDomPropertyContext context, ICodeProperty codeProperty)
        {
            return Generate(context, (T)codeProperty);

            //if (context.FieldType != null && !requiredComponentType.IsAssignableFrom(context.FieldType))
            //{
            //    context.Statements.Add(
            //        new CodeMethodInvokeExpression(
            //            new CodeFieldReferenceExpression(
            //                new CodeThisReferenceExpression(),
            //                CodeDomStructureGenerator.Names.ComponentManagerField
            //            ),
            //            componentManagerDescriptor.InitMethodName,
            //            new CodePropertyReferenceExpression(
            //                GetPropertyTarget(context),
            //                codeProperty.Property.Name
            //            )
            //        )
            //    );
            //}
        }

        protected abstract CodeStatement Generate(ICodeDomPropertyContext context, T codeProperty);
    }
}
