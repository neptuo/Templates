using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Parser.ExtensionContent;
using System.CodeDom;
using Neptuo.Web.Framework.Utils;
using System.Reflection;
using System.ComponentModel;
using Neptuo.Web.Framework.Annotations;

namespace Neptuo.Web.Framework.Compilation
{
    public class ExtensionValueGenerator : IValueCodeGenerator
    {
        public bool GenerateCode(string content, ValueGeneratorContext context)
        {
            bool parsed = false;

            Helper helper = new Helper(context);
            helper.Parser.OnParsedItem = (e) => parsed = GenerateExtension(helper, e.ParsedItem);
            helper.Parser.Parse(content);

            return parsed;
        }

        private bool GenerateExtension(Helper helper, Extension extension)
        {
            Type controlType = helper.Registrator.GetExtension(extension.Namespace.ToLowerInvariant(), extension.Name.ToLowerInvariant());
            if (controlType == null)
                return false;

            CodeObjectCreator creator = helper.Context.CodeGenerator.CreateControl()
                .Declare(controlType)
                .CreateInstance();

            BindProperties(helper, creator, extension);

            creator.CallBind(helper.Context.ParentInfo);

            if (helper.Context.ParentInfo.AsReturnStatement)
            {
                creator.AsReturnStatement(
                    helper.Context.ParentInfo, 
                    helper.Context.CodeGenerator.CreateFieldReferenceOrMethodCall(
                        creator.Field, 
                        TypeHelper.MethodName<IMarkupExtension, object>(e => e.ProvideValue)
                    )
                );
            }
            else
            {
                creator.AddToParent(helper.Context.ParentInfo, TypeHelper.MethodName<IMarkupExtension, object>(e => e.ProvideValue), true, true);
            }

            return true;
        }

        private void BindProperties(Helper helper, CodeObjectCreator creator, Extension extension)
        {
            HashSet<string> boundProperies = new HashSet<string>();
            PropertyInfo defaultProperty = ControlHelper.GetDefaultProperty(creator.FieldType);

            foreach (KeyValuePair<string, PropertyInfo> item in ControlHelper.GetProperties(creator.FieldType))
            {
                bool bound = false;
                string propertyName = item.Key.ToLowerInvariant();
                foreach (ExtensionAttribute attribute in extension.Attributes)
                {
                    if (propertyName == attribute.Name.ToLowerInvariant())
                    {
                        ParentInfo parent = helper.Context.ParentInfo;
                        helper.Context.ParentInfo = new ParentInfo(creator, item.Value.Name, null, item.Value.PropertyType);

                        helper.Context.GeneratorService.ProcessValue(attribute.Value, helper.Context);

                        helper.Context.ParentInfo = parent;

                        boundProperies.Add(propertyName);
                        bound = true;
                    }
                }

                if (!bound && item.Value != defaultProperty)
                {
                    BindPropertyDefaultValue(helper, creator, item.Value);
                    boundProperies.Add(propertyName);
                    bound = true;
                }
            }

            if (defaultProperty != null && !boundProperies.Contains(defaultProperty.Name.ToLowerInvariant()))
            {
                if (!String.IsNullOrWhiteSpace(extension.DefaultAttributeValue))
                {
                    ParentInfo parent = helper.Context.ParentInfo;
                    helper.Context.ParentInfo = new ParentInfo(creator, defaultProperty.Name, null, defaultProperty.PropertyType);

                    helper.Context.GeneratorService.ProcessValue(extension.DefaultAttributeValue, helper.Context);

                    helper.Context.ParentInfo = parent;
                }
                else
                {
                    BindPropertyDefaultValue(helper, creator, defaultProperty);
                }
            }
        }

        private void BindPropertyDefaultValue(Helper helper, CodeObjectCreator creator, PropertyInfo prop)
        {
            DependencyAttribute dependency = DependencyAttribute.GetAttribute(prop);
            if (dependency != null)
            {
                creator.SetProperty(prop.Name, helper.Context.CodeGenerator.GetDependencyFromServiceProvider(prop.PropertyType));
            }
            else
            {
                DefaultValueAttribute defaultValue = ReflectionHelper.GetAttribute<DefaultValueAttribute>(prop);
                if (defaultValue != null)
                    creator.SetProperty(prop.Name, defaultValue.Value);
            }
        }

        public class Helper
        {
            public ValueGeneratorContext Context { get; protected set; }

            public IRegistrator Registrator { get; protected set; }

            public ExtensionContentParser Parser { get; protected set; }

            public Helper(ValueGeneratorContext context)
            {
                Context = context;
                Registrator = ((IRegistrator)Context.ServiceProvider.GetService(typeof(IRegistrator)));
                Parser = new ExtensionContentParser();
            }
        }
    }
}
