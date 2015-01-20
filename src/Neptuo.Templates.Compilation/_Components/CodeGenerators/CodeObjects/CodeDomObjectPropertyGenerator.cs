using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators.CodeObjects
{
    public class XCodeDomObjectPropertyGenerator
    {
        public void GenerateCode(XCodeDomGenerator.Context context, IPropertiesCodeObject codeObject, string fieldName, CodeStatementCollection statements)
        {
            Type componentType = null;
            ITypeCodeObject typeCodeObject = codeObject as ITypeCodeObject;
            if (typeCodeObject != null)
                componentType = typeCodeObject.Type;

            foreach (ICodeProperty codeProperty in codeObject.Properties)
            {
                context.CodeGenerator.GenerateProperty(
                    new CodeDomPropertyContext(context, fieldName, componentType, statements),
                    codeProperty
                );
            }
        }
    }
}
