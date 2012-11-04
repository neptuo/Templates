using Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions;
using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators
{
    public partial class CodeDomGenerator : ICodeGenerator
    {
        public CodeDomGenerator()
        {
            CodeObjectExtensions = new Dictionary<Type, ICodeDomCodeObjectExtension>();
            SetCodeObjectExtension(typeof(ControlCodeObject), new ComponentCodeDomCodeObjectExtension());
            SetCodeObjectExtension(typeof(PlainValueCodeObject), new PlainValueCodeDomCodeObjectExtension());
            //TODO: Support for DependencyCodeObject

            PropertyDescriptorExtensions = new Dictionary<Type, ICodeDomPropertyDescriptorExtension>();
            SetPropertyDescriptorExtension(typeof(ListAddPropertyDescriptor), new ListAddCodeDomPropertyDescriptorExtension());
            SetPropertyDescriptorExtension(typeof(SetPropertyDescriptor), new SetCodeDomPropertyDescriptorExtension());
        }

        #region ICodeDomCodeObjectExtension

        //TODO: For multiple per type?
        protected Dictionary<Type, ICodeDomCodeObjectExtension> CodeObjectExtensions { get; private set; }

        public void SetCodeObjectExtension(Type type, ICodeDomCodeObjectExtension extension)
        {
            CodeObjectExtensions[type] = extension;
        }

        #endregion

        #region ICodeDomPropertyDescriptorExtension

        protected Dictionary<Type, ICodeDomPropertyDescriptorExtension> PropertyDescriptorExtensions { get; private set; }

        public void SetPropertyDescriptorExtension(Type type, ICodeDomPropertyDescriptorExtension extension)
        {
            PropertyDescriptorExtensions[type] = extension;
        }

        #endregion

        #region Name helpers

        private int fieldCount = 0;

        public string GenerateFieldName()
        {
            return String.Format("field{0}", ++fieldCount);
        }

        public string FormatBindMethod(string fieldName)
        {
            return String.Format("{0}_Bind", fieldName);
        }

        #endregion

        #region Code generation

        public CodeExpression GenerateCodeObject(ICodeObject codeObject, IPropertyDescriptor propertyDescriptor, CodeMemberMethod parentBindMethod, string parentFieldName)
        {
            foreach (KeyValuePair<Type, ICodeDomCodeObjectExtension> item in CodeObjectExtensions)
            {
                if (item.Key == codeObject.GetType())
                    return item.Value.GenerateCode(new CodeDomCodeObjectExtensionContext(this, parentBindMethod, parentFieldName), codeObject, propertyDescriptor);
            }

            throw new NotImplementedException("Not supported code object");
        }

        public void GenerateProperty(IPropertyDescriptor propertyDescriptor, string fieldName, CodeMemberMethod bindMethod)
        {
            foreach (KeyValuePair<Type, ICodeDomPropertyDescriptorExtension> item in PropertyDescriptorExtensions)
            {
                if (item.Key == propertyDescriptor.GetType())
                {
                    item.Value.GenerateProperty(new CodeDomPropertyDescriptorExtensionContext(this, fieldName, bindMethod), propertyDescriptor);
                    return;
                }
            }

            throw new NotImplementedException("Not supported property descriptor");
        }

        #endregion

        public bool ProcessTree(IPropertyDescriptor propertyDescriptor, ICodeGeneratorContext context)
        {
            CreateCodeUnit();
            CreateCodeClass();
            CreateCodeMethods();

            if (propertyDescriptor is ListAddPropertyDescriptor)
                GenerateProperty(propertyDescriptor as ListAddPropertyDescriptor, Names.ViewPageField, CreateViewPageControlsMethod);

            WriteOutput(context.Output);
            return true;
        }

        private void WriteOutput(TextWriter writer)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions
            {
                BracingStyle = "C",
                BlankLinesBetweenMembers = false,
                VerbatimOrder = false
            };

            provider.GenerateCodeFromCompileUnit(Unit, writer, options);
        }
    }
}
