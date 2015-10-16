using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Descriptors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class TypeDefaultPropertyBuilder : DefaultPropertyBuilder
    {
        protected bool IsCollectionProperty(IFieldDescriptor fieldDescriptor)
        {
            if (typeof(string) == fieldDescriptor.FieldType)
                return false;

            return typeof(IEnumerable).IsAssignableFrom(fieldDescriptor.FieldType);
        }

        protected override ICodeProperty CreateCodeProperty(IFieldDescriptor fieldDescriptor)
        {
            ICodeProperty codeProperty = null;
            if (IsCollectionProperty(fieldDescriptor))
                codeProperty = new AddCodeProperty(fieldDescriptor.Name, fieldDescriptor.FieldType);
            else
                codeProperty = new SetCodeProperty(fieldDescriptor.Name, fieldDescriptor.FieldType);

            return codeProperty;
        }
    }
}
