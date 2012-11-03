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
        partial class GeneratorHelper
        {
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

                parentBindMethod.Statements.Add(
                    new CodeAssignStatement(
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(),
                            fieldName
                        ),
                        new CodeObjectCreateExpression(component.Type)
                    )
                );

                CodeMemberMethod bindMethod = new CodeMemberMethod
                {
                    Name = FormatBindMethod(fieldName)
                };
                Class.Members.Add(bindMethod);

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

            public CodeExpression GeneratePlainValue(IPlainValueCodeObject plainValue, IPropertyDescriptor propertyDescriptor, CodeMemberMethod parentBindMethod, string parentFieldName)
            {
                return null;
            }

            public void GenerateProperty(ListAddPropertyDescriptor propertyDescriptor, string fieldName, CodeMemberMethod bindMethod)
            {
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
                                typeof(ComponentManagerCollection<>).MakeGenericType(propertyDescriptor.Property.PropertyType.GetGenericArguments()[0]),
                                new CodeFieldReferenceExpression(
                                    new CodeThisReferenceExpression(),
                                    Names.ComponentManagerField
                                ),
                                new CodePrimitiveExpression(propertyDescriptor.Property.Name),
                                new CodeFieldReferenceExpression(
                                    new CodeThisReferenceExpression(),
                                    fieldName
                                )
                            )
                        )
                    );
                }

                foreach (ICodeObject propertyValue in propertyDescriptor.Values)
                {


                    if (typeof(ICollection<>).IsAssignableFrom(propertyDescriptor.Property.PropertyType.GetGenericTypeDefinition()))
                    {
                        //bindMethod.Statements.Add(
                        //    new CodeMethodInvokeExpression(
                        //        new CodeFieldReferenceExpression(
                        //            new CodeThisReferenceExpression(),
                        //            Names.ComponentManagerField
                        //        ),
                        //        TypeHelper.MethodName<IComponentManager, object, string, object, Action>(c => c.AddComponent),
                        //        new CodeFieldReferenceExpression(
                        //            new CodeThisReferenceExpression(),
                        //            parentFieldName
                        //        ),
                        //        new CodePrimitiveExpression(propertyDescriptor.Property.Name),
                        //        new CodeFieldReferenceExpression(
                        //            new CodeThisReferenceExpression(),
                        //            fieldName
                        //        ),
                        //        new CodeMethodReferenceExpression(
                        //            new CodeThisReferenceExpression(),
                        //            bindMethod.Name
                        //        )
                        //    )
                        //);
                    }
                    GenerateCodeObject(propertyValue, propertyDescriptor, bindMethod, fieldName);
                }
            }

            public void GenerateProperty(SetPropertyDescriptor propertyDescriptor, string fieldName, CodeMemberMethod bindMethod)
            {
                GenerateCodeObject(propertyDescriptor.Value, propertyDescriptor, bindMethod, fieldName);
            }
        }
    }
}
