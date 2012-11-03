using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Utils;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    partial class CodeDomGenerator
    {
        #region Name helpers

        private int fieldCount = 0;

        protected string GenerateFieldName()
        {
            return String.Format("field{0}", ++fieldCount);
        }

        protected string FormatBindMethod(string fieldName)
        {
            return String.Format("{0}_Bind", fieldName);
        }

        #endregion

        protected CodeExpression GenerateCodeObject(ICodeObject codeObject, IPropertyDescriptor propertyDescriptor, CodeMemberMethod parentBindMethod, string parentFieldName)
        {
            if (codeObject is IComponentCodeObject)
                return GenerateComponent(codeObject as IComponentCodeObject, propertyDescriptor, parentBindMethod, parentFieldName);
            else if (codeObject is IPlainValueCodeObject)
                return GeneratePlainValue(codeObject as IPlainValueCodeObject, propertyDescriptor, parentBindMethod, parentFieldName);

            throw new NotImplementedException("Not supported codeobject");
        }

        protected CodeExpression GenerateComponent(IComponentCodeObject component, IPropertyDescriptor propertyDescriptor, CodeMemberMethod parentBindMethod, string parentFieldName)
        {
            string fieldName = GenerateFieldName();
            CodeMemberField field = new CodeMemberField(component.Type, fieldName);
            Class.Members.Add(field);

            CodeMemberMethod bindMethod = new CodeMemberMethod
            {
                Name = FormatBindMethod(fieldName)
            };
            Class.Members.Add(bindMethod);

            parentBindMethod.Statements.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        fieldName
                    ),
                    new CodeObjectCreateExpression(component.Type)
                )
            );
            parentBindMethod.Statements.Add(
                new CodeMethodInvokeExpression(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        Names.ComponentManagerField
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
            {
                if (propertyDesc is ListAddPropertyDescriptor)
                    GenerateProperty(propertyDesc as ListAddPropertyDescriptor, fieldName, bindMethod);
                else if (propertyDesc is SetPropertyDescriptor)
                    GenerateProperty(propertyDesc as SetPropertyDescriptor, fieldName, bindMethod);
            }

            return new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(),
                fieldName
            );
        }

        protected CodeExpression GeneratePlainValue(IPlainValueCodeObject plainValue, IPropertyDescriptor propertyDescriptor, CodeMemberMethod parentBindMethod, string parentFieldName)
        {


            return new CodePrimitiveExpression(plainValue.Value);
            //return null;
        }

        protected void GenerateProperty(ListAddPropertyDescriptor propertyDescriptor, string fieldName, CodeMemberMethod bindMethod)
        {
            //Create instance
            //TODO: To support IEnumerable, add casting!
            if (typeof(ICollection<>).IsAssignableFrom(propertyDescriptor.Property.PropertyType.GetGenericTypeDefinition()))
            {
                bindMethod.Statements.Add(
                    new CodeAssignStatement(
                        new CodePropertyReferenceExpression(
                            new CodeFieldReferenceExpression(
                                new CodeThisReferenceExpression(),
                                fieldName
                            ),
                            propertyDescriptor.Property.Name
                        ),
                        new CodeObjectCreateExpression(
                            typeof(List<>).MakeGenericType(propertyDescriptor.Property.PropertyType.GetGenericArguments()[0])
                        )
                    )
                );
            }

            foreach (ICodeObject propertyValue in propertyDescriptor.Values)
            {
                CodeExpression codeExpression = GenerateCodeObject(propertyValue, propertyDescriptor, bindMethod, fieldName);
                if (typeof(ICollection<>).IsAssignableFrom(propertyDescriptor.Property.PropertyType.GetGenericTypeDefinition()))
                {
                    bindMethod.Statements.Add(
                        new CodeMethodInvokeExpression(
                            new CodePropertyReferenceExpression(
                                new CodeFieldReferenceExpression(
                                    new CodeThisReferenceExpression(),
                                    fieldName
                                ),
                                propertyDescriptor.Property.Name
                            ),
                            TypeHelper.MethodName<ICollection<object>, object>(c => c.Add),
                            codeExpression
                        )
                    );
                }
                //TODO: Other bindable ways
            }
        }

        protected void GenerateProperty(SetPropertyDescriptor propertyDescriptor, string fieldName, CodeMemberMethod bindMethod)
        {
            CodeExpression codeExpression = GenerateCodeObject(propertyDescriptor.Value, propertyDescriptor, bindMethod, fieldName);

            bindMethod.Statements.Add(
                new CodeAssignStatement(
                    new CodePropertyReferenceExpression(
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(),
                            fieldName
                        ),
                        propertyDescriptor.Property.Name
                    ),
                    codeExpression
                )
            );
        }
    }
}
