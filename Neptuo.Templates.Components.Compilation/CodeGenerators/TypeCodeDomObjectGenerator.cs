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
    /// <summary>
    /// Base codeDom generator for generating class based components.
    /// </summary>
    /// <typeparam name="T">Type of code object.</typeparam>
    public abstract class TypeCodeDomObjectGenerator<T> : CodeDomObjectGeneratorBase<T>
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
        protected virtual CodeExpression GenerateCompoment<TCodeObject>(CodeObjectExtensionContext context, TCodeObject codeObject)
            where TCodeObject : ITypeCodeObject, IPropertiesCodeObject
        {
            string fieldName = GenerateFieldName();
            ComponentMethodInfo createMethod = GenerateCreateMethod(context, codeObject, fieldName);

            GenerateBindMethod(context, codeObject, fieldName, null);
            return GenerateComponentReturnExpression(context, codeObject, createMethod);
        }

        protected virtual CodeExpression GenerateComponentReturnExpression<TCodeObject>(CodeObjectExtensionContext context, TCodeObject codeObject, ComponentMethodInfo createMethod)
            where TCodeObject : ITypeCodeObject, IPropertiesCodeObject
        {
            return new CodeMethodInvokeExpression(
                new CodeThisReferenceExpression(),
                createMethod.MethodName
            );
        }

        protected virtual bool RequiresGlobalField<TCodeObject>(CodeObjectExtensionContext context, TCodeObject codeObject)
            where TCodeObject : ITypeCodeObject, IPropertiesCodeObject
        {
            return false;
        }

        /// <summary>
        /// Generates metod for creating field instance and setting default property values.
        /// </summary>
        /// <param name="context">Current context.</param>
        /// <param name="codeObject">Code object.</param>
        /// <param name="fieldName">Current field name.</param>
        /// <returns>Code method for creating instance.</returns>
        protected virtual ComponentMethodInfo GenerateCreateMethod<TCodeObject>(CodeObjectExtensionContext context, TCodeObject codeObject, string fieldName)
            where TCodeObject : ITypeCodeObject, IPropertiesCodeObject
        {
            CodeMemberMethod createMethod = new CodeMemberMethod
            {
                Name = FormatCreateMethod(fieldName),
                ReturnType = new CodeTypeReference(codeObject.Type)
            };
            context.BaseStructure.Class.Members.Add(createMethod);

            ComponentMethodInfo createMethodInfo = null;
            bool requiresGlobalField = RequiresGlobalField(context, codeObject);
            if (requiresGlobalField)
            {
                context.BaseStructure.Class.Members.Add(
                    new CodeMemberField(
                        new CodeTypeReference(codeObject.Type), 
                        fieldName
                    )
                );

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
                        CreateObjectCreateExpression(context, codeObject)
                    )
                );
                createMethodInfo = new ComponentMethodInfo(ifNull.TrueStatements, createMethod.Name, fieldName);
            }
            else
            {
                createMethod.Statements.Add(
                    new CodeVariableDeclarationStatement(
                        codeObject.Type,
                        fieldName,
                        CreateObjectCreateExpression(context, codeObject)
                    )
                );
                createMethodInfo = new ComponentMethodInfo(createMethod, fieldName);
            }

            AppendToComponentManager(context, codeObject, createMethodInfo);
            GenerateDefaultPropertyValues(context, codeObject, createMethodInfo);
            AppendToCreateMethod(context, codeObject, createMethodInfo);

            createMethod.Statements.Add(
                new CodeMethodReturnStatement(
                    new CodeFieldReferenceExpression(requiresGlobalField ? new CodeThisReferenceExpression() : null, fieldName)
                )
            );
            return createMethodInfo;
        }

        protected virtual CodeExpression CreateObjectCreateExpression<TCodeObject>(CodeObjectExtensionContext context, TCodeObject codeObject)
            where TCodeObject : ITypeCodeObject, IPropertiesCodeObject
        {
            if (context.CodeDomContext.IsDirectObjectResolve)
            {
                return context.CodeGenerator.GenerateDependency(context.CodeDomContext, codeObject.Type);
            }
            else
            {
                return new CodeObjectCreateExpression(
                    codeObject.Type,
                    ResolveConstructorParameters(context, codeObject.Type)
                );
            }
        }

        protected virtual void AppendToComponentManager<TCodeObject>(CodeObjectExtensionContext context, TCodeObject codeObject, ComponentMethodInfo createMethod)
            where TCodeObject : ITypeCodeObject, IPropertiesCodeObject
        {
            createMethod.Statements.Add(
                new CodeMethodInvokeExpression(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        CodeDomStructureGenerator.Names.ComponentManagerField
                    ),
                    TypeHelper.MethodName<IComponentManager, object, Action<object>>(m => m.AddComponent),
                    new CodeFieldReferenceExpression(
                        null,
                        createMethod.FieldName
                    ),
                    new CodeMethodReferenceExpression(
                        new CodeThisReferenceExpression(),
                        FormatBindMethod(createMethod.FieldName)
                    )
                )
            );
        }

        protected virtual void AppendToCreateMethod<TCodeObject>(CodeObjectExtensionContext context, TCodeObject codeObject, ComponentMethodInfo createMethod)
            where TCodeObject : ITypeCodeObject, IPropertiesCodeObject
        { }

        /// <summary>
        /// Appends to <paramref name="target"/> statements for setting default (not in markup set) property values.
        /// </summary>
        /// <param name="context">Current context.</param>
        /// <param name="codeObject">Code object.</param>
        /// <param name="fieldName">Current field name.</param>
        /// <param name="target">Target collection of statements.</param>
        protected virtual void GenerateDefaultPropertyValues<TCodeObject>(CodeObjectExtensionContext context, TCodeObject codeObject, ComponentMethodInfo createMethod)
            where TCodeObject : ITypeCodeObject, IPropertiesCodeObject
        {
            HashSet<string> boundProperties = new HashSet<string>(codeObject.Properties.Where(p => p.Property != null).Select(p => p.Property.Name));
            foreach (PropertyInfo propertyInfo in codeObject.Type.GetProperties())
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

                    // Try any attribute generator
                    if (targetValue == null)
                    {
                        foreach (Attribute attribute in propertyInfo.GetCustomAttributes())
                        {
                            targetValue = context.CodeGenerator.GenerateAttribute(context.CodeDomContext, propertyInfo, attribute);
                            if (targetValue != null)
                                break;
                        }
                    }

                    if (targetValue == null)
                        targetValue = context.CodeGenerator.GeneratePropertyType(context.CodeDomContext, codeObject.Type, propertyInfo);

                    // If we found value for property, generate appropriate set
                    if (targetValue != null)
                    {
                        createMethod.Statements.Add(new CodeAssignStatement(
                            new CodePropertyReferenceExpression(
                                new CodeFieldReferenceExpression(
                                    null,
                                    createMethod.FieldName
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
        protected CodeMemberMethod GenerateBindMethod<TCodeObject>(CodeObjectExtensionContext context, TCodeObject codeObject, string fieldName, string bindMethodName = null)
            where TCodeObject : ITypeCodeObject, IPropertiesCodeObject
        {
            CodeMemberMethod bindMethod = new CodeMemberMethod
            {
                Name = bindMethodName ?? FormatBindMethod(fieldName)
            };
            bindMethod.Parameters.Add(new CodeParameterDeclarationExpression(codeObject.Type, fieldName));
            context.BaseStructure.Class.Members.Add(bindMethod);

            GenerateBindMethodStatements(context, codeObject, new ComponentMethodInfo(bindMethod, fieldName));
            return bindMethod;
        }
        
        /// <summary>
        /// Generates statements for bind method.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="codeObject">Code object.</param>
        /// <param name="bindMethod">Bind method info.</param>
        protected void GenerateBindMethodStatements<TCodeObject>(CodeObjectExtensionContext context, TCodeObject codeObject, ComponentMethodInfo bindMethod)
            where TCodeObject : ITypeCodeObject, IPropertiesCodeObject
        {
            foreach (IPropertyDescriptor propertyDesc in codeObject.Properties)
            {
                context.CodeGenerator.GenerateProperty(
                    new CodeDomPropertyContext(context.CodeDomContext, bindMethod.FieldName, bindMethod.Statements),
                    propertyDesc
                );
            }
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

        public class ComponentMethodInfo
        {
            public CodeStatementCollection Statements { get; private set; }
            public string MethodName { get; private set; }
            public string FieldName { get; private set; }

            public ComponentMethodInfo(CodeMemberMethod method, string fieldName)
            {
                Statements = method.Statements;
                MethodName = method.Name;
                FieldName = fieldName;
            }

            public ComponentMethodInfo(CodeStatementCollection statements, string methodName, string fieldName)
            {
                Statements = statements;
                MethodName = methodName;
                FieldName = fieldName;
            }
        }
    }
}
