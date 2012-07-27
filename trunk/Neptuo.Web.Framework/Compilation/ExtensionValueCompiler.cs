//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Neptuo.Web.Framework.Parser.ExtensionContent;
//using System.CodeDom;
//using Neptuo.Web.Framework.Utils;
//using System.Reflection;
//using System.ComponentModel;

//namespace Neptuo.Web.Framework.Compilation
//{
//    public class ExtensionValueCompiler : IValueCompiler
//    {
//        public bool GenerateCode(string content, ValueCompilerContext context)
//        {
//            bool parsed = false;

//            Helper helper = new Helper(context);
//            helper.Parser.OnParsedItem = (e) =>
//            {
//                parsed = GenerateExtension(helper, e.ParsedItem);
//            };
//            helper.Parser.Parse(content);

//            return parsed;
//        }

//        private bool GenerateExtension(Helper helper, Extension extension)
//        {
//            Type controlType = helper.Registrator.GetExtension(extension.Namespace.ToLowerInvariant(), extension.Name.ToLowerInvariant());
//            if (controlType == null)
//                return false;

//            CodeGenerator generator = helper.Context.CodeGenerator;
//            CodeMemberField control = generator.DeclareField(controlType);
//            generator.CreateInstance(control, controlType);

//            BindProperties(helper, control, controlType, extension);
//            generator.AddToParent(helper.Context.ParentInfo, control, TypeHelper.MethodName<IMarkupExtension, object>(e => e.ProvideValue), true, true);

//            return true;
//        }

//        private void BindProperties(Helper helper, CodeMemberField control, Type controlType, Extension extension)
//        {
//            HashSet<string> boundProperies = new HashSet<string>();
//            PropertyInfo defaultProperty = ControlHelper.GetDefaultProperty(controlType);

//            foreach (KeyValuePair<string, PropertyInfo> item in ControlHelper.GetProperties(controlType))
//            {
//                bool bound = false;
//                string propertyName = item.Key.ToLowerInvariant();
//                foreach (ExtensionAttribute attribute in extension.Attributes)
//                {
//                    if (propertyName == attribute.Name.ToLowerInvariant())
//                    {
//                        ParentInfo parent = helper.Context.ParentInfo;
//                        helper.Context.ParentInfo = new ParentInfo(control, item.Value.Name, null, item.Value.PropertyType);

//                        helper.Context.CompilerService.CompileValue(attribute.Value, helper.Context);

//                        helper.Context.ParentInfo = parent;

//                        boundProperies.Add(propertyName);
//                        bound = true;
//                    }
//                }

//                if (!bound && item.Value != defaultProperty)
//                {
//                    BindPropertyDefaultValue(helper, control, item.Value);
//                    boundProperies.Add(propertyName);
//                    bound = true;
//                }
//            }

//            if (defaultProperty != null && !boundProperies.Contains(defaultProperty.Name.ToLowerInvariant()))
//            {
//                if (!String.IsNullOrWhiteSpace(extension.DefaultAttributeValue))
//                {
//                    ParentInfo parent = helper.Context.ParentInfo;
//                    helper.Context.ParentInfo = new ParentInfo(control, defaultProperty.Name, null, defaultProperty.PropertyType);

//                    helper.Context.CompilerService.CompileValue(extension.DefaultAttributeValue, helper.Context);

//                    helper.Context.ParentInfo = parent;
//                }
//                else
//                {
//                    BindPropertyDefaultValue(helper, control, defaultProperty);
//                }
//            }
//        }

//        private void BindPropertyDefaultValue(Helper helper, CodeMemberField control, PropertyInfo prop)
//        {
//            DefaultValueAttribute attr = ReflectionHelper.GetAttribute<DefaultValueAttribute>(prop);
//            if (attr != null)
//                helper.Context.CodeGenerator.SetProperty(control, prop.Name, attr.Value);
//        }
        
//        public class Helper
//        {
//            public ValueCompilerContext Context { get; protected set; }

//            public IRegistrator Registrator { get; protected set; }

//            public ExtensionContentParser Parser { get; protected set; }

//            public Helper(ValueCompilerContext context)
//            {
//                Context = context;
//                Registrator = ((IRegistrator)Context.ServiceProvider.GetService(typeof(IRegistrator)));
//                Parser = new ExtensionContentParser();
//            }
//        }
//    }
//}
