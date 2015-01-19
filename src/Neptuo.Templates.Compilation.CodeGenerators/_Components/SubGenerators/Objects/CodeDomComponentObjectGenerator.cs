using Neptuo.ComponentModel;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomComponentObjectGenerator : CodeDomObjectGeneratorBase<ComponentCodeObject>
    {
        /// <summary>
        /// If <c>true</c>, create method will contain default properties binding.
        /// </summary>
        public bool IsDefaultPropertyAssignmentInCreateMethod { get; set; }

        /// <summary>
        /// If <c>true</c>, create method will contain codeObject properties binding.
        /// </summary>
        public bool IsPropertyAssignmentInCreateMethod { get; set; }

        /// <summary>
        /// Name provider for field names.
        /// </summary>
        public IUniqueNameProvider NameProvider { get; private set; }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        public CodeDomComponentObjectGenerator(IUniqueNameProvider nameProvider)
        {
            Guard.NotNull(nameProvider, "nameProvider");
            NameProvider = nameProvider;
            IsPropertyAssignmentInCreateMethod = true;
        }

        /// <summary>
        /// Generates create method for <paramref name="codeObject"/> and returns its execution.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="codeObject">Code object to process.</param>
        /// <returns>Execution of create method for <paramref name="codeObject"/>.</returns>
        protected override ICodeDomObjectResult Generate(ICodeDomObjectContext context, ComponentCodeObject codeObject)
        {
            CodeMemberMethod createMethod = GenerateCreateMethod(context, codeObject);
            if (createMethod == null)
                return null;

            context.Structure.Class.Members.Add(createMethod);

            return new CodeDomDefaultObjectResult(
                new CodeMethodInvokeExpression(
                    new CodeThisReferenceExpression(),
                    createMethod.Name
                ),
                codeObject.Type
            );
        }

        /// <summary>
        /// Generates create method which will consist of:
        /// 1) Instance creation.
        /// 2) Binding default properties (if <see cref="CodeDomComponentObjectGenerator.IsDefaultPropertyAssignmentInCreateMethod"/> is <c>true</c>).
        /// 3) Binding properties from codeObject (if <see cref="CodeDomComponentObjectGenerator.IsPropertyAssignmentInCreateMethod"/> is <c>true</c>).
        /// 4) Result of <see cref="CodeDomComponentObjectGenerator.GenerateCreateMethodAdditionalStatements"/>.
        /// 5) Return statement.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="codeObject">Code object to process.</param>
        /// <returns>Create method for <paramref name="codeObject"/>.</returns>
        protected virtual CodeMemberMethod GenerateCreateMethod(ICodeDomObjectContext context, ComponentCodeObject codeObject)
        {
            // Get field name.
            string fieldName = NameProvider.Next();

            // Create method which 
            CodeMemberMethod createMethod = new CodeMemberMethod()
            {
                Name = FormatUniqueName(fieldName, "Create"),
                Attributes = MemberAttributes.Private,
                ReturnType = new CodeTypeReference(codeObject.Type)
            };

            // 1) Create new instance.
            IEnumerable<CodeStatement> statements = GenerateInstanceCreation(context, codeObject, fieldName);
            if (statements == null)
                return null;

            createMethod.Statements.AddRange(statements);

            // If create method should contain default property values.
            if (IsDefaultPropertyAssignmentInCreateMethod)
            {
                // 2) Bind default properties.
                statements = GenerateDefaultPropertyAssignment(context, codeObject, fieldName);
                if (statements == null)
                    return null;

                createMethod.Statements.AddRange(statements);
            }

            // If create method should contain codeObject property values.
            if (IsPropertyAssignmentInCreateMethod)
            {
                // 3) Bind properties from code object.
                statements = GeneratePropertyAssignment(context, codeObject, fieldName);
                if (statements == null)
                    return null;

                createMethod.Statements.AddRange(statements);
            }

            // 4) Append additional statements to create method.
            statements = GenerateCreateMethodAdditionalStatements(context, codeObject, fieldName);
            if (statements == null)
                return null;

            createMethod.Statements.AddRange(statements);

            // 5) Append return statement.
            createMethod.Statements.Add(new CodeMethodReturnStatement(
                new CodeVariableReferenceExpression(fieldName)
            ));
            return createMethod;
        }

        /// <summary>
        /// Provides enumeration of statements for creating instance of type represented by <paramref name="codeObject"/>.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="codeObject">Code object to process.</param>
        /// <param name="fieldName">Field name for <paramref name="codeObject"/>.</param>
        /// <returns>Enumeration of statements for creating instance of type represented by <paramref name="codeObject"/>.</returns>
        protected IEnumerable<CodeStatement> GenerateInstanceCreation(ICodeDomObjectContext context, ComponentCodeObject codeObject, string fieldName)
        {
            CodeDomNewInstanceFeature instanceGenerator = new CodeDomNewInstanceFeature();
            CodeExpression instanceExpression = instanceGenerator.Generate(context, codeObject.Type);
            if (instanceExpression == null)
                return null;

            return new List<CodeStatement>() 
            {
                new CodeVariableDeclarationStatement(
                    new CodeTypeReference(codeObject.Type),
                    fieldName,
                    instanceExpression
                )
            };
        }

        /// <summary>
        /// Provides enumeration of statements for default properties defined on type of <paramref name="codeObject"/>.
        /// Currently uses <see cref="CodeDomPropertyDefaultValueFeature"/> to generate default value based on attributes.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="codeObject">Code object to process.</param>
        /// <param name="fieldName">Field name for <paramref name="codeObject"/>.</param>
        /// <returns>Enumeration of statements for default properties defined on type of <paramref name="codeObject"/>.</returns>
        protected IEnumerable<CodeStatement> GenerateDefaultPropertyAssignment(ICodeDomObjectContext context, ComponentCodeObject codeObject, string fieldName)
        {
            List<CodeStatement> statements = new List<CodeStatement>();
            CodeDomPropertyDefaultValueFeature generator = new CodeDomPropertyDefaultValueFeature();

            HashSet<string> boundProperties = new HashSet<string>(codeObject.Properties.Select(p => p.Property.Name));
            foreach (PropertyInfo propertyInfo in codeObject.Type.GetProperties())
            {
                if (!boundProperties.Contains(propertyInfo.Name))
                {
                    CodeExpression expression;
                    if (!generator.TryGenerate(context, propertyInfo, out expression))
                        return null;

                    if (expression != null)
                    {
                        statements.Add(new CodeAssignStatement(
                            new CodePropertyReferenceExpression(
                                new CodeVariableReferenceExpression(fieldName),
                                propertyInfo.Name
                            ),
                            expression
                        ));
                    }
                }
            }

            return statements;
        }

        /// <summary>
        /// Provides enumeration of statements for properties set on <paramref name="codeObject"/>.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="codeObject">Code object to process.</param>
        /// <param name="fieldName">Field name for <paramref name="codeObject"/>.</param>
        /// <returns>Enumeration of statements for properties set on <paramref name="codeObject"/>.</returns>
        protected IEnumerable<CodeStatement> GeneratePropertyAssignment(ICodeDomObjectContext context, ComponentCodeObject codeObject, string fieldName)
        {
            CodeDomAstPropertyFeature propertyGenerator = new CodeDomAstPropertyFeature();
            IEnumerable<CodeStatement> statements = propertyGenerator.Generate(context, codeObject, fieldName);
            return statements;
        }

        /// <summary>
        /// When overriden in derived class, can return statements which will be added before return statement from creat method.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="codeObject">Code object to process.</param>
        /// <param name="fieldName">Field name for <paramref name="codeObject"/>.</param>
        /// <returns>Additional statements for create method.</returns>
        protected IEnumerable<CodeStatement> GenerateCreateMethodAdditionalStatements(ICodeDomObjectContext context, ComponentCodeObject codeObject, string fieldName)
        {
            return Enumerable.Empty<CodeStatement>();
        }

        /// <summary>
        /// Using defined conventions, formats member name for <paramref name="fieldName"/> and <paramref name="memberName"/>.
        /// </summary>
        /// <param name="fieldName">Field name (unique prefix).</param>
        /// <param name="memberName">Custom memeber name.</param>
        /// <returns>Formatted member name for <paramref name="fieldName"/> and <paramref name="memberName"/>.</returns>
        protected string FormatUniqueName(string fieldName, string memberName)
        {
            return String.Format("{0}_{1}", fieldName, memberName);
        }
    }
}
