using Neptuo;
using Neptuo.Activators;
using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeGenerators;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.Compilation.CodeGenerators
{
    public class CodeDomDependencyProviderGenerator : ICodeDomDependencyGenerator
    {
        public CodeExpression Generate(ICodeDomContext context, Type type)
        {
            context.Structure.Unit.Namespaces[0].Imports.Add(new CodeNamespaceImport(typeof(IDependencyProvider).Namespace));

            return new CodeMethodInvokeExpression(
                new CodeMethodReferenceExpression(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        "dependencyProvider"
                    ),
                    TypeHelper.MethodName<IDependencyProvider, Type, object>(p => p.Resolve),
                    new CodeTypeReference(type)
                )
            );
        }
    }
}
