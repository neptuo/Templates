using Neptuo.Identifiers;
using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Implementation of object generator for <see cref="IObserverCodeObject"/> using component wrap
    /// and delegation to <see cref="CodeDomComponentObjectGenerator"/>.
    /// Takes instance of <see cref="IObserverCodeObject"/>, wraps it to <see cref="XComponentCodeObject"/>
    /// and generates code using <see cref="CodeDomComponentObjectGenerator"/>.
    /// </summary>
    public class CodeDomObserverObjectGenerator : CodeDomControlObjectGenerator
    {
        public CodeDomObserverObjectGenerator(IUniqueNameProvider nameProvider)
            : base(nameProvider)
        { }

        public override ICodeDomObjectResult Generate(ICodeDomObjectContext context, ICodeObject codeObject)
        {
            ITypeCodeObject typeCodeObject = codeObject as ITypeCodeObject;
            if (typeCodeObject == null)
            {
                context.AddError("Unnable to generate code for observer, which is not ITypeCodeObject.");
                return null;
            }

            IFieldCollectionCodeObject fields = codeObject as IFieldCollectionCodeObject;
            if (fields == null)
            {
                context.AddError("Unnable to generate code for observer, which is not IPropertiesCodeObject.");
                return null;
            }

            XComponentCodeObject component = new XComponentCodeObject(typeCodeObject.Type);
            foreach (ICodeProperty codeProperty in fields.EnumerateProperties())
                component.AddProperty(codeProperty);
            
            return base.Generate(context, component);
        }

        protected override ICodeDomObjectResult Generate(ICodeDomObjectContext context, XComponentCodeObject codeObject, string fieldName)
        {
            ICodeDomObjectResult result = base.Generate(context, codeObject, fieldName);
            if (result == null)
                return result;

            string variableName;
            if (!context.TryGetObserverTarget(out variableName))
            {
                context.AddError("Unnable to process observer without target variable name.");
                return null;
            }

            if (!result.HasExpression())
            {
                context.AddError("Unnable to process observer without expression from generated component.");
                return null;
            }

            return new CodeDomDefaultObjectResult(
                new CodeExpressionStatement(
                    new CodeMethodInvokeExpression(
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(),
                            "componentManager"
                        ),
                        TypeHelper.MethodName<IComponentManager, IControl, IControlObserver, Action<IControlObserver>>(m => m.AttachObserver),
                        new CodeVariableReferenceExpression(variableName),
                        result.Expression,
                        new CodeMethodReferenceExpression(
                            new CodeThisReferenceExpression(),
                            FormatUniqueName(fieldName, BindMethodSuffix)
                        )
                    )
                )
            );
        }
    }
}
