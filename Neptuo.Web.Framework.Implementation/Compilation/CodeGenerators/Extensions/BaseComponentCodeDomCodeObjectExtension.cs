using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions
{
    public abstract class BaseComponentCodeDomCodeObjectExtension<T> : BaseCodeDomCodeObjectExtension<T>
        where T : ICodeObject
    {
        /// <summary>
        /// Generates base 'stuff' for component (including field, bind method, properties).
        /// Doesn't generate Control/Observer/Extension specific calls.
        /// </summary>
        protected CodeFieldReferenceExpression GenerateCompoment(CodeDomCodeObjectExtensionContext context, ITypeCodeObject typeCodeObject, IPropertiesCodeObject propertiesCodeObject)
        {
            string fieldName = context.CodeGenerator.GenerateFieldName();
            CodeMemberField field = new CodeMemberField(typeCodeObject.Type, fieldName);
            context.CodeGenerator.Class.Members.Add(field);

            CodeMemberMethod bindMethod = new CodeMemberMethod
            {
                Name = context.CodeGenerator.FormatBindMethod(fieldName)
            };
            context.CodeGenerator.Class.Members.Add(bindMethod);

            context.ParentBindMethod.Statements.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        fieldName
                    ),
                    new CodeObjectCreateExpression(typeCodeObject.Type)
                )
            );

            foreach (IPropertyDescriptor propertyDesc in propertiesCodeObject.Properties)
                context.CodeGenerator.GenerateProperty(propertyDesc, fieldName, bindMethod);

            return new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(),
                fieldName
            );
        }
    }
}
