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

        public CodeObjectCreator Declare(Type controlType, Type bindReturnType = null)
        {
            FieldType = controlType;
            string fieldName = generator.GenerateFieldName();

            Field = new CodeMemberField
            {
                Name = fieldName,
                Attributes = MemberAttributes.Private,
                Type = new CodeTypeReference(controlType)
            };
            generator.Class.Members.Add(Field);

            CreateBindMethod(bindReturnType);

            return this;
        }

        public CodeObjectCreator CreateInstance(params CodeExpression[] parameters)
        {
            generator.CreateControlsMethod.Statements.Add(new CodeAssignStatement(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    Field.Name
                ),
                new CodeObjectCreateExpression(
                    new CodeTypeReference(FieldType),
                    parameters
                )
            ));

            return this;
        }

        public CodeObjectCreator AsReturnStatement(ParentInfo parent, CodeExpression result)
        {
            parent.Creator.BindMethod.Statements.Add(new CodeMethodReturnStatement(
                result
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

        public CodeObjectCreator CallBind(ParentInfo parent)
        {
            parent.Creator.BindMethod.Statements.Add(
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(
                        new CodeThisReferenceExpression(),
                        BindMethod.Name
                    )
                )
            );

            return this;
        }

        public CodeObjectCreator AddToParent(ParentInfo parent, string fieldMethodName = null, bool cast = false, bool inBindMethod = false)
        {
            CodeMemberMethod method = generator.CreateControlsMethod;
            if (inBindMethod || parent.AsReturnStatement)
                method = parent.Creator.BindMethod;

            if (parent.MethodName != null)
            {
                method.Statements.Add(new CodeMethodInvokeExpression(
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
                method.Statements.Add(new CodeAssignStatement(
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

        public CodeObjectCreator RegisterObserver(string attributeName, CodeObjectCreator observerCreator)
        {
            generator.InitMethod.Statements.Add(new CodeMethodInvokeExpression(
                new CodeMethodReferenceExpression(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        CodeGenerator.Names.LivecycleObserverField
                    ),
                    TypeHelper.MethodName<ILivecycleObserver, object, IObserver, string, Func<object>>(l => l.RegisterObserver)
                ),
                new CodeVariableReferenceExpression(Field.Name),
                new CodeVariableReferenceExpression(observerCreator.Field.Name),
                new CodePrimitiveExpression(attributeName),
                new CodeVariableReferenceExpression(observerCreator.BindMethod.Name)
            ));

            return this;
        }

        public CodeObjectCreator CreateBindMethod(Type bindReturnType = null)
        {
            BindMethod = CreateBindMethod(Field.Name, FieldType, generator, bindReturnType);
            return this;
        }

        public static CodeMemberMethod CreateBindMethod(string fieldName, Type controlType, CodeGenerator generator, Type bindReturnType = null)
        {
            CodeMemberMethod bindMethod = new CodeMemberMethod
            {
                Name = String.Format("{0}_Bind", fieldName),
                Attributes = MemberAttributes.Private | MemberAttributes.Final,
            };

            if (bindReturnType != null)
                bindMethod.ReturnType = new CodeTypeReference(bindReturnType);

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
