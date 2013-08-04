using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public abstract class TypeCodeDomObjectGenerator<T> : BaseCodeDomObjectGenerator<T>
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
            context.CodeDomContext.BaseStructure.Class.Members.Add(field);


            CodeMemberMethod createMethod = GenerateCreateMethod(context, typeCodeObject, propertiesCodeObject, fieldName);

            context.ParentBindMethod.Statements.Add(    
                new CodeMethodInvokeExpression(
                    new CodeThisReferenceExpression(),
                    createMethod.Name
                )
            );

            GenerateBindMethod(context, propertiesCodeObject, fieldName, null);

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
            context.CodeDomContext.BaseStructure.Class.Members.Add(createMethod);

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
                    new CodeObjectCreateExpression(typeCodeObject.Type, ResolveConstructorParameters(context, typeCodeObject.Type))
                )
            );

            CodeMemberMethod fakeMethod = new CodeMemberMethod();
            foreach (IPropertyDescriptor propertyDesc in propertiesCodeObject.Properties)
            {
                IDefaultPropertyValue defaultProperty = propertyDesc as IDefaultPropertyValue;
                if (defaultProperty != null && defaultProperty.IsDefaultValue)
                    context.CodeGenerator.GenerateProperty(context.CodeDomContext, propertyDesc, fieldName, fakeMethod);
            }

            foreach (CodeStatement statement in fakeMethod.Statements)
                ifNull.TrueStatements.Add(statement);

            return createMethod;
        }

        /// <summary>
        /// Generates and fill bind method using <code>IPropertiesCodeObject.Properties</code>.
        /// </summary>
        protected CodeMemberMethod GenerateBindMethod(CodeObjectExtensionContext context, IPropertiesCodeObject codeObject, string fieldName, string bindMethodName = null)
        {
            CodeMemberMethod bindMethod = new CodeMemberMethod
            {
                Name = bindMethodName ?? context.CodeGenerator.FormatBindMethod(fieldName)
            };
            context.CodeDomContext.BaseStructure.Class.Members.Add(bindMethod);

            foreach (IPropertyDescriptor propertyDesc in codeObject.Properties)
            {
                IDefaultPropertyValue defaultProperty = propertyDesc as IDefaultPropertyValue;
                if (defaultProperty == null || !defaultProperty.IsDefaultValue)
                    context.CodeGenerator.GenerateProperty(context.CodeDomContext, propertyDesc, fieldName, bindMethod);
            }

            return bindMethod;
        }

        protected CodeExpression[] ResolveConstructorParameters(CodeObjectExtensionContext context, Type type)
        {
            //TODO: This as metadata for extension...
            List<CodeExpression> result = new List<CodeExpression>();

            ConstructorInfo ctor = type.GetConstructors().OrderBy(c => c.GetParameters().Count()).FirstOrDefault();
            if (ctor != null)
            {
                foreach (ParameterInfo parameter in ctor.GetParameters())
                {
                    //if (parameter.ParameterType == typeof(IComponentManager))
                    //    result.Add(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), BaseStructureExtension.Names.ComponentManagerField));
                    //else if (parameter.ParameterType == typeof(IDependencyProvider))
                    //    result.Add(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), BaseStructureExtension.Names.DependencyProviderField));
                    //else if (parameter.ParameterType == typeof(IViewPage))
                    //    result.Add(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), BaseStructureExtension.Names.ViewPageField));
                    //else
                    //{
                    //    result.Add(new CodeCastExpression(
                    //        new CodeTypeReference(parameter.ParameterType),
                    //        new CodeMethodInvokeExpression(
                    //        new CodeFieldReferenceExpression(
                    //            new CodeThisReferenceExpression(),
                    //            BaseStructureExtension.Names.DependencyProviderField
                    //        ),
                    //        TypeHelper.MethodName<IDependencyProvider, Type, string, object>(p => p.Resolve),
                    //        new CodeTypeOfExpression(new CodeTypeReference(parameter.ParameterType)),
                    //        new CodePrimitiveExpression(null)
                    //    )));
                    //}
                    CodeExpression parameterExpression = context.CodeGenerator.GenerateDependency(context.CodeDomContext, parameter.ParameterType);
                    if (parameterExpression != null)
                        result.Add(parameterExpression);
                    else
                        throw new NotImplementedException("Not supported parameter type!");
                }
            }

            return result.ToArray();
        }
    }
}
