using Neptuo.Templates.Compilation.CodeGenerators.Extensions;
using Neptuo.Templates.Compilation.CodeGenerators.Extensions.CodeDom;
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
            CodeObjectExtensions = new Dictionary<Type, ICodeObjectExtension>();
            PropertyDescriptorExtensions = new Dictionary<Type, IPropertyDescriptorExtension>();
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

        public CodeExpression GenerateCodeObject(Context context, ICodeObject codeObject, IPropertyDescriptor propertyDescriptor, CodeMemberMethod parentBindMethod, string parentFieldName)
        {
            foreach (KeyValuePair<Type, ICodeObjectExtension> item in CodeObjectExtensions)
            {
                if (item.Key == codeObject.GetType())
                    return item.Value.GenerateCode(new CodeObjectExtensionContext(context, parentBindMethod, parentFieldName), codeObject, propertyDescriptor);
            }

            throw new NotImplementedException("Not supported code object");
        }

        public void GenerateProperty(Context context, IPropertyDescriptor propertyDescriptor, string fieldName, CodeMemberMethod bindMethod)
        {
            foreach (KeyValuePair<Type, IPropertyDescriptorExtension> item in PropertyDescriptorExtensions)
            {
                if (item.Key == propertyDescriptor.GetType())
                {
                    item.Value.GenerateProperty(new PropertyDescriptorExtensionContext(context, fieldName, bindMethod), propertyDescriptor);
                    return;
                }
            }

            throw new NotImplementedException("Not supported property descriptor");
        }

        #endregion

        public bool ProcessTree(IPropertyDescriptor propertyDescriptor, ICodeGeneratorContext codeContext)
        {
            Context context = new Context(codeContext, Names.ClassName, this);

            CreateCodeUnit(context);
            CreateCodeClass(context);
            CreateCodeMethods(context);

            if (propertyDescriptor is ListAddPropertyDescriptor)
                GenerateProperty(context, propertyDescriptor as ListAddPropertyDescriptor, Names.ViewPageField, context.CreateViewPageControlsMethod);

            WriteOutput(context.Unit, codeContext.Output);
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
