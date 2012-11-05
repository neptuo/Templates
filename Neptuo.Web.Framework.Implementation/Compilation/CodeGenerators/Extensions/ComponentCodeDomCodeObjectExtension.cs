using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Utils;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions
{
    public class ComponentCodeDomCodeObjectExtension : BaseCodeDomCodeObjectExtension<IComponentCodeObject>
    {
        protected override CodeExpression GenerateCode(CodeDomCodeObjectExtensionContext context, IComponentCodeObject component, IPropertyDescriptor propertyDescriptor)
        {
            string fieldName = context.CodeGenerator.GenerateFieldName();
            CodeMemberField field = new CodeMemberField(component.Type, fieldName);
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
                    new CodeObjectCreateExpression(component.Type)
                )
            );
            context.ParentBindMethod.Statements.Add(
                new CodeMethodInvokeExpression(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        CodeDomGenerator.Names.ComponentManagerField
                    ),
                    TypeHelper.MethodName<IComponentManager, object, Action>(m => m.AddComponent),
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        fieldName
                    ),
                    new CodeMethodReferenceExpression(
                        new CodeThisReferenceExpression(),
                        bindMethod.Name
                    )
                )
            );

            foreach (IPropertyDescriptor propertyDesc in component.Properties)
                context.CodeGenerator.GenerateProperty(propertyDesc, fieldName, bindMethod);

            return new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(),
                fieldName
            );
        }

        protected void AttachObservers(CodeDomCodeObjectExtensionContext context, IComponentCodeObject component)
        {

        }
    }
}
