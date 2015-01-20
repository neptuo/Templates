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
    public partial class XCodeDomGenerator : ICodeGenerator
    {
        public bool IsDirectObjectResolve { get; set; }

        public XCodeDomGenerator()
        {
            CodeObjectGenerators = new Dictionary<Type, ICodeDomComponentGenerator>();
            CodePropertyGenerators = new Dictionary<Type, XICodeDomPropertyGenerator>();
            DependencyProviderGenerators = new Dictionary<Type, XICodeDomDependencyGenerator>();
            AttributeGenerators = new Dictionary<Type, XICodeDomAttributeGenerator>();
            PropertyTypeGenerators = new Dictionary<Type, XICodeDomPropertyTypeGenerator>();
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

        protected Dictionary<Type, XICodeDomPropertyGenerator> CodePropertyGenerators { get; private set; }

        public void SetCodePropertyGenerator(Type type, XICodeDomPropertyGenerator generator)
        {
            CodePropertyGenerators[type] = generator;
        }

        #endregion

        #region ICodeDomDependencyGenerator

        protected Dictionary<Type, XICodeDomDependencyGenerator> DependencyProviderGenerators { get; private set; }

        public void SetDependencyProviderGenerator(Type type, XICodeDomDependencyGenerator generator)
        {
            DependencyProviderGenerators[type] = generator;
        }

        #endregion

        #region ICodeDomAttributeGenerator

        protected Dictionary<Type, XICodeDomAttributeGenerator> AttributeGenerators { get; private set; }

        public void SetAttributeGenerator(Type type, XICodeDomAttributeGenerator generator)
        {
            AttributeGenerators[type] = generator;
        }

        #endregion

        #region ICodeDomPropertyTypeGenerator

        protected Dictionary<Type, XICodeDomPropertyTypeGenerator> PropertyTypeGenerators { get; private set; }

        public void SetPropertyTypeGenerator(Type type, XICodeDomPropertyTypeGenerator generator)
        {
            PropertyTypeGenerators[type] = generator;
        }

        #endregion

        #region ICodeDomBaseStructureExtension

        public XICodeDomStructureGenerator BaseStructureGenerator { get; private set; }

        public void SetBaseStructureGenerator(XICodeDomStructureGenerator generator)
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

        public CodeExpression GenerateCodeObject(CodeObjectExtensionContext context, ICodeObject codeObject, ICodeProperty codeProperty)
        {
            Guard.NotNull(context, "context");
            Guard.NotNull(codeObject, "codeObject");
            foreach (KeyValuePair<Type, ICodeDomComponentGenerator> item in CodeObjectGenerators)
            {
                if (item.Key == codeObject.GetType())
                    return item.Value.GenerateCode(context, codeObject, codeProperty);
            }

            throw new NotImplementedException(String.Format("Not supported code object of type '{0}'", codeObject.GetType()));
        }

        public void GenerateProperty(CodeDomPropertyContext context, ICodeProperty codeProperty)
        {
            foreach (KeyValuePair<Type, XICodeDomPropertyGenerator> item in CodePropertyGenerators)
            {
                if (item.Key == codeProperty.GetType())
                {
                    item.Value.GenerateProperty(context, codeProperty);
                    return;
                }
            }

            throw new NotImplementedException("Not supported property descriptor!");
        }

        public CodeExpression GenerateDependency(Context context, Type type)
        {
            foreach (KeyValuePair<Type, XICodeDomDependencyGenerator> item in DependencyProviderGenerators)
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

        public CodeExpression GenerateAttribute(Context context, PropertyInfo propertyInfo, Attribute attribute)
        {
            foreach (KeyValuePair<Type, XICodeDomAttributeGenerator> item in AttributeGenerators)
            {
                if (item.Key.IsAssignableFrom(attribute.GetType()))
                {
                    CodeExpression expression = item.Value.GenerateCode(new CodeDomAttributeContext(context, propertyInfo), attribute);
                    if (expression != null)
                        return expression;
                }
            }
            return null;
        }

        public CodeExpression GeneratePropertyType(Context context, Type type, PropertyInfo propertyInfo)
        {
            foreach (KeyValuePair<Type, XICodeDomPropertyTypeGenerator> item in PropertyTypeGenerators)
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

        public bool ProcessTree(ICodeObject codeObject, ICodeGeneratorContext codeContext)
        {
            Guard.NotNull(codeObject, "codeObject");
            Guard.NotNull(codeContext, "codeContext");
            Context context = new Context(codeContext, this, IsDirectObjectResolve);

            if (BaseStructureGenerator == null)
                throw new ArgumentNullException("BaseStructureExtension", "Base structure extension not provided!");

            INaming naming = context.CodeGeneratorContext.DependencyProvider.Resolve<INaming>();
            context.Structure = BaseStructureGenerator.GenerateCode(new CodeDomStructureContext(context, naming));

            GenerateCodeObject(
                new CodeObjectExtensionContext(context, null),
                codeObject,
                null
            );
            RunCodeDomVisitors(context);

            WriteOutput(context.Structure.Unit, codeContext.Output);
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
