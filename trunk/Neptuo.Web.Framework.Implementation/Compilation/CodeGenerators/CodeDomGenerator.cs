using Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions;
using Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions.CodeDom;
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
            CodeObjectExtensions = new Dictionary<Type, ICodeObjectExtension>();
            SetCodeObjectExtension(typeof(ControlCodeObject), new ComponentCodeObjectExtension());
            SetCodeObjectExtension(typeof(PlainValueCodeObject), new PlainValueCodeObjectExtension());
            SetCodeObjectExtension(typeof(LocalFieldCodeObject), new LocalFieldCodeObjectExtension());
            SetCodeObjectExtension(typeof(DependencyCodeObject), new DependencyCodeObjectExtension());

            PropertyDescriptorExtensions = new Dictionary<Type, IPropertyDescriptorExtension>();
            SetPropertyDescriptorExtension(typeof(ListAddPropertyDescriptor), new ListAddPropertyDescriptorExtension());
            SetPropertyDescriptorExtension(typeof(SetPropertyDescriptor), new SetPropertyDescriptorExtension());
            SetPropertyDescriptorExtension(typeof(MethodInvokePropertyDescriptor), new MethodInvokePropertyDescriptorExtension());
        }

        #region ICodeDomCodeObjectExtension

        //TODO: For multiple per type?
        protected Dictionary<Type, ICodeObjectExtension> CodeObjectExtensions { get; private set; }

        public void SetCodeObjectExtension(Type type, ICodeObjectExtension extension)
        {
            CodeObjectExtensions[type] = extension;
        }

        #endregion

        #region ICodeDomPropertyDescriptorExtension

        protected Dictionary<Type, IPropertyDescriptorExtension> PropertyDescriptorExtensions { get; private set; }

        public void SetPropertyDescriptorExtension(Type type, IPropertyDescriptorExtension extension)
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

        public string FormatCreateMethod(string fieldName)
        {
            return String.Format("{0}_Create", fieldName);
        }

        #endregion

        #region Code generation

        public CodeExpression GenerateCodeObject(IDependencyProvider dependencyProvider, ICodeObject codeObject, IPropertyDescriptor propertyDescriptor, CodeMemberMethod parentBindMethod, string parentFieldName)
        {
            foreach (KeyValuePair<Type, ICodeObjectExtension> item in CodeObjectExtensions)
            {
                if (item.Key == codeObject.GetType())
                    return item.Value.GenerateCode(new CodeObjectExtensionContext(dependencyProvider, this, parentBindMethod, parentFieldName), codeObject, propertyDescriptor);
            }

            throw new NotImplementedException("Not supported code object");
        }

        public void GenerateProperty(IDependencyProvider dependencyProvider, IPropertyDescriptor propertyDescriptor, string fieldName, CodeMemberMethod bindMethod)
        {
            foreach (KeyValuePair<Type, IPropertyDescriptorExtension> item in PropertyDescriptorExtensions)
            {
                if (item.Key == propertyDescriptor.GetType())
                {
                    item.Value.GenerateProperty(new PropertyDescriptorExtensionContext(dependencyProvider, this, fieldName, bindMethod), propertyDescriptor);
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
                GenerateProperty(context.DependencyProvider, propertyDescriptor as ListAddPropertyDescriptor, Names.ViewPageField, CreateViewPageControlsMethod);

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
