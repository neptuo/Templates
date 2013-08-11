using Neptuo.Linq.Expressions;
using Neptuo.Reflection;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public abstract class TypeCodeDomObjectGenerator<T> : BaseCodeDomObjectGenerator<T> 
        where T : ICodeObject
    {
        #region Name helpers

        protected IFieldNameProvider FieldNameProvider { get; private set; }

        public string GenerateFieldName()
        {
            return FieldNameProvider.GetName();
        }

        public string FormatBindMethod(string fieldName)
        {
            return String.Format("{0}_Bind", fieldName);
        }

        public string FormatCreateMethod(string fieldName)
        {
            return String.Format("{0}_Create", fieldName);
        }

        #endregion

        public TypeCodeDomObjectGenerator(IFieldNameProvider fieldNameProvider)
        {
            FieldNameProvider = fieldNameProvider;
        }

        /// <summary>
        /// Generates base 'stuff' for component (including field, bind method, properties).
        /// Doesn't generate Control/Observer/Extension specific calls.
        /// </summary>
        protected CodeFieldReferenceExpression GenerateCompoment(CodeObjectExtensionContext context, ITypeCodeObject typeCodeObject, IPropertiesCodeObject propertiesCodeObject)
        {
            string fieldName = GenerateFieldName();
            CodeMemberField field = new CodeMemberField(typeCodeObject.Type, fieldName);
            context.BaseStructure.Class.Members.Add(field);


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

        /// <summary>
        /// Generates metod for creating field instance and setting default property values.
        /// </summary>
        /// <param name="context">Current context.</param>
        /// <param name="typeCodeObject">Type code object.</param>
        /// <param name="propertiesCodeObject">Properties code object.</param>
        /// <param name="fieldName">Current field name.</param>
        /// <returns>Code method for creating instance.</returns>
        protected CodeMemberMethod GenerateCreateMethod(CodeObjectExtensionContext context, ITypeCodeObject typeCodeObject, IPropertiesCodeObject propertiesCodeObject, string fieldName)
        {
            CodeMemberMethod createMethod = new CodeMemberMethod
            {
                Name = FormatCreateMethod(fieldName)
            };
            context.BaseStructure.Class.Members.Add(createMethod);

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

            GenerateDefaultPropertyValues(context, typeCodeObject, propertiesCodeObject, fieldName, ifNull.TrueStatements);
            return createMethod;
        }

        /// <summary>
        /// Appends to <paramref name="target"/> statements for setting default (not in markup set) property values.
        /// </summary>
        /// <param name="context">Current context.</param>
        /// <param name="typeCodeObject">Type code object.</param>
        /// <param name="propertiesCodeObject">Properties code object.</param>
        /// <param name="fieldName">Current field name.</param>
        /// <param name="target">Target collection of statements.</param>
        protected virtual void GenerateDefaultPropertyValues(CodeObjectExtensionContext context, ITypeCodeObject typeCodeObject, IPropertiesCodeObject propertiesCodeObject, string fieldName, CodeStatementCollection target)
        {
            HashSet<string> boundProperties = new HashSet<string>(propertiesCodeObject.Properties.Select(p => p.Property.Name));
            foreach (PropertyInfo propertyInfo in typeCodeObject.Type.GetProperties())
            {
                if (propertyInfo.CanWrite && boundProperties.Add(propertyInfo.Name))
                {
                    CodeExpression targetValue = null;

                    // DefaultValueAttribute
                    if (targetValue == null)
                    {
                        DefaultValueAttribute defaultValue = ReflectionHelper.GetAttribute<DefaultValueAttribute>(propertyInfo);
                        if (defaultValue != null)
                            targetValue = new CodePrimitiveExpression(defaultValue.Value);
                    }

                    // DependencyAttribute
                    if (targetValue == null)
                    {
                        DependencyAttribute dependency = ReflectionHelper.GetAttribute<DependencyAttribute>(propertyInfo);
                        if (dependency != null)
                            targetValue = context.CodeGenerator.GenerateDependency(context.CodeDomContext, propertyInfo.PropertyType);
                    }

                    // If we found value for property, generate appropriate set
                    if (targetValue != null)
                    {
                        target.Add(new CodeAssignStatement(
                            new CodePropertyReferenceExpression(
                                new CodeFieldReferenceExpression(
                                    new CodeThisReferenceExpression(),
                                    fieldName
                                ),
                                propertyInfo.Name
                            ),
                            targetValue
                        ));
                    }
                }
            }
        }

        /// <summary>
        /// Generates and fill bind method using <code>IPropertiesCodeObject.Properties</code>.
        /// </summary>
        protected CodeMemberMethod GenerateBindMethod(CodeObjectExtensionContext context, IPropertiesCodeObject codeObject, string fieldName, string bindMethodName = null)
        {
            CodeMemberMethod bindMethod = new CodeMemberMethod
            {
                Name = bindMethodName ?? FormatBindMethod(fieldName)
            };
            context.BaseStructure.Class.Members.Add(bindMethod);

            foreach (IPropertyDescriptor propertyDesc in codeObject.Properties)
                context.CodeGenerator.GenerateProperty(context.CodeDomContext, propertyDesc, fieldName, bindMethod);

            return bindMethod;
        }

        /// <summary>
        /// Resolves all constructor parameters (using <code>CodeGenerator.GenerateDependency</code>).
        /// </summary>
        /// <param name="context">Current context.</param>
        /// <param name="type">Target control type to resolve.</param>
        /// <returns>Generated code expressions.</returns>
        protected CodeExpression[] ResolveConstructorParameters(CodeObjectExtensionContext context, Type type)
        {
            List<CodeExpression> result = new List<CodeExpression>();
            ConstructorInfo ctor = type.GetConstructors().OrderBy(c => c.GetParameters().Count()).FirstOrDefault();
            if (ctor != null)
            {
                foreach (ParameterInfo parameter in ctor.GetParameters())
                {
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
