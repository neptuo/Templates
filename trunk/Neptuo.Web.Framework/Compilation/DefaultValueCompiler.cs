using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Utils;
using TypeConverter = Neptuo.Web.Framework.Utils.TypeConverter;

namespace Neptuo.Web.Framework.Compilation
{
    public class DefaultValueCompiler : IValueCompiler
    {
        public bool GenerateCode(string content, ValueCompilerContext context)
        {
            CodeGenerator generator = context.CodeGenerator;

            if (TypeConverter.CanConvert(context.ParentInfo.RequiredType))
            {
                if (context.ParentInfo.AsReturnStatement)
                    context.ParentInfo.Creator.AsReturnStatement(context.ParentInfo, new CodePrimitiveExpression(content));
                else
                    context.ParentInfo.Creator.SetProperty(context.ParentInfo.PropertyName, TypeConverter.Convert(content, context.ParentInfo.RequiredType));

                return true;
            }

            BindPropertyDefaultValue(context);
            return true;
        }

        private void BindPropertyDefaultValue(ValueCompilerContext context)
        {
            DefaultValueAttribute attr = ReflectionHelper.GetAttribute<DefaultValueAttribute>(context.ParentInfo.RequiredType);
            if (attr != null)
                context.ParentInfo.Creator.SetProperty(context.ParentInfo.PropertyName, attr.Value);
        }
    }
}
