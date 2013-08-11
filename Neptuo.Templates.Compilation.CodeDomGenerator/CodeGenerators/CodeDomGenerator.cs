﻿using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.CodeObjects;
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

        #region ICodeDomBaseStructureExtension

        public ICodeDomStructureGenerator BaseStructureGenerator { get; private set; }

        public void SetBaseStructureGenerator(ICodeDomStructureGenerator generator)
        {
            BaseStructureGenerator = generator;
        }

        #endregion

        #region Code generation

        public CodeExpression GenerateCodeObject(Context context, ICodeObject codeObject, IPropertyDescriptor propertyDescriptor, CodeMemberMethod parentBindMethod, string parentFieldName)
        {
            foreach (KeyValuePair<Type, ICodeDomComponentGenerator> item in CodeObjectGenerators)
            {
                if (item.Key == codeObject.GetType())
                    return item.Value.GenerateCode(new CodeObjectExtensionContext(context, parentBindMethod, parentFieldName), codeObject, propertyDescriptor);
            }

            throw new NotImplementedException("Not supported code object");
        }

        public void GenerateProperty(Context context, IPropertyDescriptor propertyDescriptor, string fieldName, CodeMemberMethod bindMethod)
        {
            foreach (KeyValuePair<Type, ICodeDomPropertyGenerator> item in PropertyDescriptorGenerators)
            {
                if (item.Key == propertyDescriptor.GetType())
                {
                    item.Value.GenerateProperty(new CodeDomPropertyContext(context, fieldName, bindMethod), propertyDescriptor);
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

            GenerateProperty(context, propertyDescriptor, null, context.BaseStructure.EntryPointMethod);

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