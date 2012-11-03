using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public partial class CodeDomGenerator : ICodeGenerator, IExtensibleCodeGenerator
    {
        public bool ProcessTree(IPropertyDescriptor propertyDescriptor, ICodeGeneratorContext context)
        {
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

        public void AddExtension(Type codeObjectType, Type extensionCodeGeneratorType)
        {
            throw new NotImplementedException();
        }
    }
}
