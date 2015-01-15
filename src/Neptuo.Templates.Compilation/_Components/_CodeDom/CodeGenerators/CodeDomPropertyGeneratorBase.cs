﻿using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Base implementation of <see cref="ICodeDomPropertyGenerator"/> which casts input property descriptor to <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of property descriptor that is supported.</typeparam>
    public abstract class CodeDomPropertyGeneratorBase<T> : ICodeDomPropertyGenerator
        where T : ICodeProperty
    {
        private readonly Type requiredComponentType;

        public CodeDomPropertyGeneratorBase(Type requiredComponentType)
        {
            Guard.NotNull(requiredComponentType, "requiredComponentType");
            this.requiredComponentType = requiredComponentType;
        }

        public void GenerateProperty(CodeDomPropertyContext context, ICodeProperty codeProperty)
        {
            GenerateProperty(context, (T)codeProperty);

            if (context.FieldType != null && !requiredComponentType.IsAssignableFrom(context.FieldType))
            {
                context.Statements.Add(
                    new CodeMethodInvokeExpression(
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(),
                            CodeDomStructureGenerator.Names.ComponentManagerField
                        ),
                        "Init",
                        new CodePropertyReferenceExpression(
                            GetPropertyTarget(context),
                            codeProperty.Property.Name
                        )
                    )
                );
            }
        }

        protected abstract void GenerateProperty(CodeDomPropertyContext context, T codeProperty);

        /// <summary>
        /// Creates expression that can be used to set value of property described in <paramref name="context"/>.
        /// Returns <see cref="CodeThisReferenceExpression"/> if field name is not specified; or <see cref="CodeFieldReferenceExpression"/> of <code>context.FieldName</code>.
        /// </summary>
        /// <param name="context">Current code generation context.</param>
        /// <returns>Expression that can be used to set value of property described in <paramref name="context"/>.</returns>
        protected CodeExpression GetPropertyTarget(CodeDomPropertyContext context)
        {
            if (context.FieldName != null)
            {
                return new CodeFieldReferenceExpression(
                    null,
                    context.FieldName
                );
            }
                
            return new CodeThisReferenceExpression();
        }
    }
}
