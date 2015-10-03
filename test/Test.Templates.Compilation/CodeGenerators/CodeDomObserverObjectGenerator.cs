﻿using Neptuo.Identifiers;
using Neptuo.Linq.Expressions;
using Neptuo.Models.Features;
using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.CodeObjects.Features;
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
            IComponentCodeObject source = (IComponentCodeObject)codeObject;

            ITypeCodeObject type;
            if (!source.TryWith(out type))
            {
                context.AddError("Unnable to generate code for observer, which is not ITypeCodeObject.");
                return null;
            }

            IFieldCollectionCodeObject fields;
            if (!source.TryWith(out fields))
            {
                context.AddError("Unnable to generate code for observer, which is not IPropertiesCodeObject.");
                return null;
            }

            return base.Generate(context, source);
        }

        protected override ICodeDomObjectResult Generate(ICodeDomObjectContext context, IComponentCodeObject codeObject, string fieldName)
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
