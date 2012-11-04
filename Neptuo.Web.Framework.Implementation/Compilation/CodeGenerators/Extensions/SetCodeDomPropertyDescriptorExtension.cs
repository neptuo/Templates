﻿using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions
{
    public class SetCodeDomPropertyDescriptorExtension : BaseCodeDomPropertyDescriptorExtension<SetPropertyDescriptor>
    {
        protected override void GenerateProperty(CodeDomPropertyDescriptorExtensionContext context, SetPropertyDescriptor propertyDescriptor)
        {
            CodeExpression codeExpression = context.CodeGenerator.GenerateCodeObject(propertyDescriptor.Value, propertyDescriptor, context.BindMethod, context.FieldName);
            context.BindMethod.Statements.Add(
                new CodeAssignStatement(
                    new CodePropertyReferenceExpression(
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(),
                            context.FieldName
                        ),
                        propertyDescriptor.Property.Name
                    ),
                    codeExpression
                )
            );
        }
    }
}
