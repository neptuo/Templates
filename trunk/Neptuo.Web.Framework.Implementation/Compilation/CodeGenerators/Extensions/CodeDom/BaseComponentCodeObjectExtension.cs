using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions.CodeDom
{
    public abstract class BaseComponentCodeObjectExtension<T> : BaseCodeObjectExtension<T>
        where T : ICodeObject
    {
        /// <summary>
        /// Generates base 'stuff' for component (including field, bind method, properties).
        /// Doesn't generate Control/Observer/Extension specific calls.
        /// </summary>
        protected CodeFieldReferenceExpression GenerateCompoment(CodeObjectExtensionContext context, ITypeCodeObject typeCodeObject, IPropertiesCodeObject propertiesCodeObject)
        {
            string fieldName = context.CodeGenerator.GenerateFieldName();
            CodeMemberField field = new CodeMemberField(typeCodeObject.Type, fieldName);
            context.CodeGenerator.Class.Members.Add(field);


            CodeMemberMethod createMethod = GenerateCreateMethod(context, typeCodeObject, propertiesCodeObject, fieldName);

            context.ParentBindMethod.Statements.Add(    
                new CodeMethodInvokeExpression(
                    new CodeThisReferenceExpression(),
                    createMethod.Name
                )
            );

            GenerateBindMethod(context, propertiesCodeObject, fieldName, null, true);

            return new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(),
                fieldName
            );
        }

        protected CodeMemberMethod GenerateCreateMethod(CodeObjectExtensionContext context, ITypeCodeObject typeCodeObject, IPropertiesCodeObject propertiesCodeObject, string fieldName)
        {
            CodeMemberMethod createMethod = new CodeMemberMethod
            {
                Name = context.CodeGenerator.FormatCreateMethod(fieldName)
            };
            context.CodeGenerator.Class.Members.Add(createMethod);

            CodeConditionStatement ifNull = new CodeConditionStatement
            {
                Condition = new CodeBinaryOperatorExpression(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        fieldName
                    ),
                    CodeBinaryOperatorType.ValueEquality,
                    new CodePrimitiveExpression(null)
                )
            };
            createMethod.Statements.Add(ifNull);
            ifNull.TrueStatements.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        fieldName
                    ),
                    new CodeObjectCreateExpression(typeCodeObject.Type)
                )
            );

            CodeMemberMethod fakeMethod = new CodeMemberMethod();
            foreach (IPropertyDescriptor propertyDesc in propertiesCodeObject.Properties)
            {
                IDefaultPropertyValue defaultProperty = propertyDesc as IDefaultPropertyValue;
                if (defaultProperty != null && defaultProperty.IsDefaultValue)
                    context.CodeGenerator.GenerateProperty(context.DependencyProvider, propertyDesc, fieldName, fakeMethod);
            }

            foreach (CodeStatement statement in fakeMethod.Statements)
                ifNull.TrueStatements.Add(statement);

            return createMethod;
        }

        /// <summary>
        /// Generates and fill bind method using <code>IPropertiesCodeObject.Properties</code>.
        /// </summary>
        protected CodeMemberMethod GenerateBindMethod(CodeObjectExtensionContext context, IPropertiesCodeObject codeObject, string fieldName, string bindMethodName = null, bool addDefaultProperties = false)
        {
            CodeMemberMethod bindMethod = new CodeMemberMethod
            {
                Name = bindMethodName ?? context.CodeGenerator.FormatBindMethod(fieldName)
            };
            context.CodeGenerator.Class.Members.Add(bindMethod);

            foreach (IPropertyDescriptor propertyDesc in codeObject.Properties)
            {
                IDefaultPropertyValue defaultProperty = propertyDesc as IDefaultPropertyValue;
                if (defaultProperty == null || addDefaultProperties || !defaultProperty.IsDefaultValue)
                    context.CodeGenerator.GenerateProperty(context.DependencyProvider, propertyDesc, fieldName, bindMethod);
            }

            return bindMethod;
        }
    }
}
