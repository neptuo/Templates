using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.PostProcessing;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public partial class CodeDomGenerator : ICodeGenerator
    {
        public CodeDomGenerator()
        {
            CodeObjectGenerators = new Dictionary<Type, ICodeDomComponentGenerator>();
            PropertyDescriptorGenerators = new Dictionary<Type, ICodeDomPropertyGenerator>();
            DependencyProviderGenerators = new Dictionary<Type, ICodeDomDependencyGenerator>();
            AttributeGenerators = new Dictionary<Type, ICodeDomAttributeGenerator>();
            PropertyTypeGenerators = new Dictionary<Type, ICodeDomPropertyTypeGenerator>();
            CodeDomVisitors = new HashSet<ICodeDomVisitor>();
        }

        #region ICodeDomComponentGenerator

        //TODO: For multiple per type?
        protected Dictionary<Type, ICodeDomComponentGenerator> CodeObjectGenerators { get; private set; }

        public void SetCodeObjectGenerator(Type type, ICodeDomComponentGenerator generator)
        {
            CodeObjectGenerators[type] = generator;
        }

        #endregion

        #region ICodeDomPropertyGenerator

        protected Dictionary<Type, ICodeDomPropertyGenerator> PropertyDescriptorGenerators { get; private set; }

        public void SetPropertyDescriptorGenerator(Type type, ICodeDomPropertyGenerator generator)
        {
            PropertyDescriptorGenerators[type] = generator;
        }

        #endregion

        #region ICodeDomDependencyGenerator

        protected Dictionary<Type, ICodeDomDependencyGenerator> DependencyProviderGenerators { get; private set; }

        public void SetDependencyProviderGenerator(Type type, ICodeDomDependencyGenerator generator)
        {
            DependencyProviderGenerators[type] = generator;
        }

        #endregion

        #region ICodeDomAttributeGenerator

        protected Dictionary<Type, ICodeDomAttributeGenerator> AttributeGenerators { get; private set; }

        public void SetAttributeGenerator(Type type, ICodeDomAttributeGenerator generator)
        {
            AttributeGenerators[type] = generator;
        }

        #endregion

        #region ICodeDomPropertyTypeGenerator

        protected Dictionary<Type, ICodeDomPropertyTypeGenerator> PropertyTypeGenerators { get; private set; }

        public void SetPropertyTypeGenerator(Type type, ICodeDomPropertyTypeGenerator generator)
        {
            PropertyTypeGenerators[type] = generator;
        }

        #endregion

        #region ICodeDomBaseStructureExtension

        public ICodeDomStructureGenerator BaseStructureGenerator { get; private set; }

        public void SetBaseStructureGenerator(ICodeDomStructureGenerator generator)
        {
            BaseStructureGenerator = generator;
        }

        #endregion

        #region ICodeDomVisitor

        protected HashSet<ICodeDomVisitor> CodeDomVisitors { get; private set; }

        public void AddCodeDomVisitor(ICodeDomVisitor visitor)
        {
            CodeDomVisitors.Add(visitor);
        }

        protected void RunCodeDomVisitors(Context context)
        {
            if (CodeDomVisitors.Any())
            {
                ICodeDomVisitorContext visitorContext = new CodeDomVisitorContext(context);
                foreach (ICodeDomVisitor visitor in CodeDomVisitors)
                    visitor.Visit(visitorContext);
            }
        }

        #endregion

        #region Code generation

        public CodeExpression GenerateCodeObject(CodeObjectExtensionContext context, ICodeObject codeObject, IPropertyDescriptor propertyDescriptor)
        {
            foreach (KeyValuePair<Type, ICodeDomComponentGenerator> item in CodeObjectGenerators)
            {
                if (item.Key == codeObject.GetType())
                    return item.Value.GenerateCode(context, codeObject, propertyDescriptor);
            }

            throw new NotImplementedException("Not supported code object");
        }

        public void GenerateProperty(CodeDomPropertyContext context, IPropertyDescriptor propertyDescriptor)
        {
            foreach (KeyValuePair<Type, ICodeDomPropertyGenerator> item in PropertyDescriptorGenerators)
            {
                if (item.Key == propertyDescriptor.GetType())
                {
                    item.Value.GenerateProperty(context, propertyDescriptor);
                    return;
                }
            }

            throw new NotImplementedException("Not supported property descriptor!");
        }

        public CodeExpression GenerateDependency(Context context, Type type)
        {
            foreach (KeyValuePair<Type, ICodeDomDependencyGenerator> item in DependencyProviderGenerators)
            {
                if (item.Key.IsAssignableFrom(type))
                {
                    CodeExpression expression = item.Value.GenerateCode(new CodeDomDependencyContext(context), type);
                    if (expression != null)
                        return expression;
                }
            }

            throw new NotImplementedException("Not supported type for dependency resolution!");
        }

        public CodeExpression GenerateAttribute(Context context, Attribute attribute)
        {
            foreach (KeyValuePair<Type, ICodeDomAttributeGenerator> item in AttributeGenerators)
            {
                if (item.Key.IsAssignableFrom(attribute.GetType()))
                {
                    CodeExpression expression = item.Value.GenerateCode(new CodeDomAttributeContext(context), attribute);
                    if (expression != null)
                        return expression;
                }
            }
            return null;
        }

        public CodeExpression GeneratePropertyType(Context context, Type type, PropertyInfo propertyInfo)
        {
            foreach (KeyValuePair<Type, ICodeDomPropertyTypeGenerator> item in PropertyTypeGenerators)
            {
                if (item.Key.IsAssignableFrom(propertyInfo.PropertyType))
                {
                    CodeExpression expression = item.Value.GenerateCode(new CodeDomPropertyTypeGeneratorContext(context), type, propertyInfo);
                    if (expression != null)
                        return expression;
                }
            }
            return null;
        }

        #endregion

        public bool ProcessTree(IPropertyDescriptor propertyDescriptor, ICodeGeneratorContext codeContext)
        {
            Context context = new Context(codeContext, this);

            if (BaseStructureGenerator == null)
                throw new ArgumentNullException("BaseStructureExtension", "Base structure extension not provided!");

            INamingContext namingContext = codeContext as INamingContext;
            if(namingContext == null)
                throw new ArgumentNullException("codeContext", "CodeDomGenerator requires INamingContext!");

            context.BaseStructure = BaseStructureGenerator.GenerateCode(new CodeDomStructureContext(context, namingContext.Naming));

            GenerateProperty(
                new CodeDomPropertyContext(context, null, context.BaseStructure.EntryPointMethod.Statements), 
                propertyDescriptor
            );
            RunCodeDomVisitors(context);

            WriteOutput(context.BaseStructure.Unit, codeContext.Output);
            return true;
        }

        private void WriteOutput(CodeCompileUnit unit, TextWriter writer)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions
            {
                BracingStyle = "C",
                BlankLinesBetweenMembers = false,
                VerbatimOrder = false
            };

            provider.GenerateCodeFromCompileUnit(unit, writer, options);
        }
    }
}
