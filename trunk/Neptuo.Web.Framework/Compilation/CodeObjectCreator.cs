using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Utils;

namespace Neptuo.Web.Framework.Compilation
{
    public class CodeObjectCreator
    {
        private CodeGenerator generator;

        public Type FieldType { get; set; }
        public CodeMemberField Field { get; set; }
        public CodeMemberMethod BindMethod { get; set; }

        public CodeObjectCreator(CodeGenerator generator)
        {
            this.generator = generator;
        }

        public CodeObjectCreator Declare(Type controlType)
        {
            FieldType = controlType;
            string fieldName = generator.GenerateFieldName();

            BindMethod = CreateBindMethod(fieldName, controlType, generator);

            Field = new CodeMemberField
            {
                Name = fieldName,
                Attributes = MemberAttributes.Private,
                Type = new CodeTypeReference(controlType)
            };
            generator.Class.Members.Add(Field);

            return this;
        }

        public CodeObjectCreator CreateInstance()
        {
            generator.CreateControlsMethod.Statements.Add(new CodeAssignStatement(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    Field.Name
                ),
                new CodeObjectCreateExpression(
                    new CodeTypeReference(FieldType)
                )
            ));

            return this;
        }

        public CodeObjectCreator SetProperty(string propertyName, CodeExpression assign)
        {
            BindMethod.Statements.Add(new CodeAssignStatement(
                new CodePropertyReferenceExpression(
                    new CodeVariableReferenceExpression(Field.Name),
                    propertyName
                ),
                assign
            ));

            return this;
        }

        public CodeObjectCreator SetProperty(string propertyName, object value)
        {
            return SetProperty(propertyName, new CodePrimitiveExpression(value));
        }

        public CodeObjectCreator AddToParent(ParentInfo parent, string fieldMethodName = null, bool cast = false)
        {
            if (parent.MethodName != null)
            {
                generator.CreateControlsMethod.Statements.Add(new CodeMethodInvokeExpression(
                    new CodeFieldReferenceExpression(
                        new CodePropertyReferenceExpression(
                            new CodeThisReferenceExpression(),
                            parent.Creator.Field.Name
                        ),
                        parent.PropertyName
                    ),
                    parent.MethodName,
                    generator.CreateFieldReferenceOrMethodCall(Field, fieldMethodName, cast ? parent.RequiredType : null)
                ));
            }
            else
            {
                generator.CreateControlsMethod.Statements.Add(new CodeAssignStatement(
                    new CodeFieldReferenceExpression(
                        new CodePropertyReferenceExpression(
                            new CodeThisReferenceExpression(),
                            parent.Creator.Field.Name
                        ),
                        parent.PropertyName
                    ),
                    generator.CreateFieldReferenceOrMethodCall(Field, fieldMethodName, cast ? parent.RequiredType : null)
                ));
            }

            return this;
        }

        public CodeObjectCreator RegisterLivecycleObserver(ParentInfo parent)
        {
            generator.InitMethod.Statements.Add(new CodeMethodInvokeExpression(
                new CodeMethodReferenceExpression(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        CodeGenerator.Names.LivecycleObserverField
                    ),
                    TypeHelper.MethodName<ILivecycleObserver, object, Action<object>>(l => l.Register)
                ),
                new CodeVariableReferenceExpression(parent.Creator.Field.Name),
                new CodeVariableReferenceExpression(Field.Name),
                new CodeVariableReferenceExpression(BindMethod.Name)
            ));


            return this;
        }



        public static CodeMemberMethod CreateBindMethod(string fieldName, Type controlType, CodeGenerator generator)
        {
            CodeMemberMethod bindMethod = new CodeMemberMethod
            {
                Name = String.Format("Bind_{0}", fieldName),
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            //bindMethod.Parameters.Add(new CodeParameterDeclarationExpression
            //{
            //    Name = fieldName,
            //    Type = new CodeTypeReference(typeof(object))
            //});
            generator.Class.Members.Add(bindMethod);
            return bindMethod;
        }
    }
}
