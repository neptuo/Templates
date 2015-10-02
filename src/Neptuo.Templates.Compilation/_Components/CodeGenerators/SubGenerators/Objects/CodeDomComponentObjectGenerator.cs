using Neptuo.Identifiers;
using Neptuo.Models.Features;
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
    public class CodeDomComponentObjectGenerator : CodeDomObjectGeneratorBase<IComponentCodeObject>
    {
        /// <summary>
        /// Suffix of 'create' method.
        /// </summary>
        protected const string CreateMethodSuffix = "Create";

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
            Ensure.NotNull(nameProvider, "nameProvider");
            NameProvider = nameProvider;
            IsPropertyAssignmentInCreateMethod = true;
        }

        /// <summary>
        /// Generates create method for <paramref name="codeObject"/> and returns its execution.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="codeObject">Code object to process.</param>
        /// <returns>Execution of create method for <paramref name="codeObject"/>.</returns>
        protected override ICodeDomObjectResult Generate(ICodeDomObjectContext context, IComponentCodeObject codeObject)
        {
            // Get field name.
            string fieldName = NameProvider.Next();

            // Generate with unique field name.
            return Generate(context, codeObject, fieldName);
        }

        /// <summary>
        /// Generates create method for <paramref name="codeObject"/> and returns its execution.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="codeObject">Code object to process.</param>
        /// <param name="fieldName">Field name for <paramref name="codeObject"/>.</param>
        /// <returns>Execution of create method for <paramref name="codeObject"/>.</returns>
        protected virtual ICodeDomObjectResult Generate(ICodeDomObjectContext context, IComponentCodeObject codeObject, string fieldName)
        {
            // Generate create method.
            CodeMemberMethod createMethod = GenerateCreateMethod(context, codeObject, fieldName);
            if (createMethod == null)
                return null;

            // Append to structure.
            context.Structure.Class.Members.Add(createMethod);

            // Return the invocation of create method.
            return new CodeDomDefaultObjectResult(
                new CodeMethodInvokeExpression(
                    new CodeThisReferenceExpression(),
                    createMethod.Name
                ),
                codeObject.With<ITypeCodeObject>().Type
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
        /// <param name="fieldName">Field name for <paramref name="codeObject"/>.</param>
        /// <returns>Create method for <paramref name="codeObject"/>.</returns>
        protected virtual CodeMemberMethod GenerateCreateMethod(ICodeDomObjectContext context, IComponentCodeObject codeObject, string fieldName)
        {
            // Create method.
            CodeMemberMethod createMethod = new CodeMemberMethod()
            {
                Name = FormatUniqueName(fieldName, CreateMethodSuffix),
                Attributes = MemberAttributes.Private,
                ReturnType = new CodeTypeReference(codeObject.With<ITypeCodeObject>().Type)
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
                statements = GenerateAstPropertyAssignment(context, codeObject, fieldName);
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
        protected virtual IEnumerable<CodeStatement> GenerateInstanceCreation(ICodeDomObjectContext context, IComponentCodeObject codeObject, string fieldName)
        {
            CodeDomNewInstanceFeature instanceGenerator = new CodeDomNewInstanceFeature();
            CodeExpression instanceExpression = instanceGenerator.Generate(context, codeObject.With<ITypeCodeObject>().Type);
            if (instanceExpression == null)
                return null;

            return new List<CodeStatement>() 
            {
                new CodeVariableDeclarationStatement(
                    new CodeTypeReference(codeObject.With<ITypeCodeObject>().Type),
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
        protected virtual IEnumerable<CodeStatement> GenerateDefaultPropertyAssignment(ICodeDomObjectContext context, IComponentCodeObject codeObject, string fieldName)
        {
            List<CodeStatement> statements = new List<CodeStatement>();
            CodeDomPropertyDefaultValueFeature generator = new CodeDomPropertyDefaultValueFeature();

            HashSet<string> boundProperties = new HashSet<string>(codeObject.With<IFieldCollectionCodeObject>().EnumerateProperties().Select(p => p.Name));
            foreach (PropertyInfo propertyInfo in codeObject.With<ITypeCodeObject>().Type.GetProperties())
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
        protected virtual IEnumerable<CodeStatement> GenerateAstPropertyAssignment(ICodeDomObjectContext context, IComponentCodeObject codeObject, string fieldName)
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
        protected virtual IEnumerable<CodeStatement> GenerateCreateMethodAdditionalStatements(ICodeDomObjectContext context, IComponentCodeObject codeObject, string fieldName)
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
