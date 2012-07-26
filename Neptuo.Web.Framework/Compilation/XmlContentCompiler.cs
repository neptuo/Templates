using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Neptuo.Web.Framework.Annotations;
using Neptuo.Web.Framework.Controls;
using Neptuo.Web.Framework.Utils;
using TypeConverter = Neptuo.Web.Framework.Utils.TypeConverter;

namespace Neptuo.Web.Framework.Compilation
{
    public class XmlContentCompiler : IContentCompiler
    {
        public bool GenerateCode(string content, ContentCompilerContext context)
        {
            Helper helper = new Helper(content, context);

            GenerateRecursive(helper, helper.Document.DocumentElement.ChildNodes.ToEnumerable());
            AppendPlainText(helper.Content.ToString(), helper.Context);

            return true;
        }

        private void GenerateRecursive(Helper helper, IEnumerable<XmlNode> childNodes)
        {
            foreach (XmlNode node in childNodes)
            {
                if (NeedsServerProcessing(node))
                {
                    AppendPlainText(helper.Content.ToString(), helper.Context);
                    helper.Content.Clear();

                    if (node.GetType() == typeof(XmlElement))
                    {
                        XmlElement element = node as XmlElement;
                        Type controlType = helper.Registrator.GetControl(element.Prefix, element.LocalName);

                        if (controlType != null)
                        {
                            ControlBuilderAttribute attr = ControlBuilderAttribute.GetAttribute(controlType);
                            if (attr != null)
                            {
                                IXmlControlBuilder builder = Activator.CreateInstance(attr.BuilderType) as IXmlControlBuilder;
                                if (builder != null)
                                {
                                    builder.GenerateControl(controlType, element, new XmlBuilderContext(helper.Context, this));
                                    return;
                                }
                            }

                            GenerateControl(helper, controlType, element);
                        }
                    }
                }
                else
                {
                    if (node.GetType() == typeof(XmlElement))
                    {
                        XmlElement element = node as XmlElement;
                        if (element.IsEmpty)
                        {
                            helper.FormatEmptyElement(element);
                        }
                        else
                        {
                            helper.FormatStartElement(element);
                            GenerateRecursive(helper, element.ChildNodes.ToEnumerable());
                            helper.FormatEndElement(element);
                        }
                    }
                    else if (node.GetType() == typeof(XmlText))
                    {
                        XmlText text = node as XmlText;
                        helper.Content.Append(text.InnerText);
                    }
                }
            }
        }

        private bool NeedsServerProcessing(XmlNode node)
        {
            XmlElement element = node as XmlElement;
            return element != null && !String.IsNullOrWhiteSpace(element.Prefix);
        }

        private void AppendPlainText(string text, ContentCompilerContext context)
        {
            bool canUse = context.ParentInfo.RequiredType.IsAssignableFrom(typeof(LiteralControl));

            if (!canUse)
                return;

            if (String.IsNullOrWhiteSpace(text))
                return;

            text = text.Trim();

            CodeGenerator generator = context.CodeGenerator;

            CodeMemberField declareText = generator.DeclareField(typeof(LiteralControl));
            generator.CreateInstance(declareText, typeof(LiteralControl));
            generator.SetProperty(declareText, "Text", text);
            generator.AddToParent(context.ParentInfo, declareText);

            if (typeof(LiteralControl).IsAssignableFrom(typeof(IDisposable)))
                generator.InvokeDisposeMethod(declareText);
        }

        public void GenerateControl(Helper helper, Type controlType, XmlElement element)
        {
            CodeGenerator generator = helper.Context.CodeGenerator;

            CodeMemberField control = generator.DeclareField(controlType);
            generator.CreateInstance(control, controlType);

            BindProperties(helper, control, controlType, element);

            generator.AddToParent(helper.Context.ParentInfo, control);

            if (controlType.IsAssignableFrom(typeof(IDisposable)))
                generator.InvokeDisposeMethod(control);
        }

        private void BindProperties(Helper helper, CodeMemberField control, Type controlType, XmlElement element)
        {
            CodeGenerator generator = helper.Context.CodeGenerator;
            HashSet<string> boundProperies = new HashSet<string>();
            PropertyInfo defaultProperty = ControlHelper.GetDefaultProperty(controlType);

            foreach (KeyValuePair<string, PropertyInfo> item in ControlHelper.GetProperties(controlType))
            {
                bool bound = false;
                string propertyName = item.Key.ToLowerInvariant();
                foreach (XmlAttribute attribute in element.Attributes)
                {
                    if (propertyName == attribute.Name.ToLowerInvariant())
                    {
                        ParentInfo parent = helper.Context.ParentInfo;
                        helper.Context.ParentInfo = new ParentInfo(control, item.Value.Name, null, item.Value.PropertyType);
                        
                        helper.Context.CompilerService.CompileValue(attribute.Value, helper.Context);
                        
                        helper.Context.ParentInfo = parent;

                        boundProperies.Add(propertyName);
                        bound = true;
                    }
                }

                XmlNode child;
                if (!bound && FindChildNode(element, propertyName, out child))
                {
                    ResolvePropertyValue(helper, control, controlType, item.Value, child.ChildNodes.ToEnumerable());
                    //ResolvePropertyValue(control, controlType, context, item.Value, args.ParsedItem.Content);
                    boundProperies.Add(propertyName);
                    bound = true;
                }

                if (!bound && item.Value != defaultProperty)
                {
                    BindPropertyDefaultValue(helper, control, item.Value);
                    boundProperies.Add(propertyName);
                    bound = true;
                }
            }

            if (defaultProperty != null && !boundProperies.Contains(defaultProperty.Name.ToLowerInvariant()))
            {
                IEnumerable<XmlNode> defaultChildNodes = FindNotUsedChildNodes(element, boundProperies);
                if (defaultChildNodes.Any())
                    ResolvePropertyValue(helper, control, controlType, defaultProperty, defaultChildNodes);
                else
                    BindPropertyDefaultValue(helper, control, defaultProperty);
            }
        }

        private bool FindChildNode(XmlElement element, string name, out XmlNode result)
        {
            foreach (XmlNode node in element.ChildNodes)
            {
                if (node.Name.ToLowerInvariant() == name)
                {
                    result = node;
                    return true;
                }
            }

            result = null;
            return false;
        }

        private IEnumerable<XmlNode> FindNotUsedChildNodes(XmlElement element, HashSet<string> usedNodes)
        {
            foreach (XmlNode node in element.ChildNodes)
            {
                if (!usedNodes.Contains(node.Name.ToLowerInvariant()))
                    yield return node;
            }
        }

        private void BindPropertyDefaultValue(Helper helper, CodeMemberField control, PropertyInfo prop)
        {
            DefaultValueAttribute attr = ReflectionHelper.GetAttribute<DefaultValueAttribute>(prop);
            if (attr != null)
                helper.Context.CodeGenerator.SetProperty(control, prop.Name, attr.Value);
        }

        private void ResolvePropertyValue(Helper helper, CodeMemberField control, Type controlType, PropertyInfo prop, IEnumerable<XmlNode> content)
        {
            CodeGenerator generator = helper.Context.CodeGenerator;
            if (ReflectionHelper.IsGenericType<IList>(prop.PropertyType))
            {
                ParentInfo parent = helper.Context.ParentInfo;
                helper.Context.ParentInfo = new ParentInfo(control, prop.Name, "Add", ReflectionHelper.GetGenericArgument(prop.PropertyType));

                //TODO: Run ...
                GenerateRecursive(helper, content);

                helper.Context.ParentInfo = parent;
            }
            else if (TypeConverter.CanConvert(prop.PropertyType))
            {
                StringBuilder contentValue = new StringBuilder();
                foreach (XmlNode node in content)
                    contentValue.Append(node.OuterXml);

                generator.SetProperty(control, prop.Name, TypeConverter.Convert(contentValue.ToString(), prop.PropertyType));
            }
            else
            {
                ParentInfo parent = helper.Context.ParentInfo;
                helper.Context.ParentInfo = new ParentInfo(control, prop.Name, null, prop.PropertyType);

                //TODO: Run ...
                GenerateRecursive(helper, content);

                helper.Context.ParentInfo = parent;
            }
        }

        public class Helper
        {
            public ContentCompilerContext Context { get; protected set; }

            public XmlDocument Document { get; protected set; }

            public IRegistrator Registrator { get; protected set; }

            public StringBuilder Content { get; protected set; }

            public Helper(string xml, ContentCompilerContext context)
            {
                Context = context;
                Registrator = ((IRegistrator)Context.ServiceProvider.GetService(typeof(IRegistrator)));
                Content = new StringBuilder();

                if (xml != null)
                {
                    Document = new XmlDocument();
                    Document.LoadXml(CreateRootElement(xml));
                }
            }

            public void FormatEmptyElement(XmlElement element)
            {
                Content.AppendFormat("<{0}", element.Name);
                foreach (XmlAttribute attribute in element.Attributes)
                    Content.AppendFormat(" {0}=\"{1}\"", attribute.Name, attribute.Value);

                Content.Append(" />");
            }

            public void FormatStartElement(XmlElement element)
            {
                Content.AppendFormat("<{0}", element.Name);
                foreach (XmlAttribute attribute in element.Attributes)
                    Content.AppendFormat(" {0}=\"{1}\"", attribute.Name, attribute.Value);

                Content.Append(">");
            }

            public void FormatEndElement(XmlElement element)
            {
                Content.AppendFormat("</{0}>", element.Name);
            }

            private string CreateRootElement(string content)
            {
                HashSet<string> usedPrefixes = new HashSet<string>();
                StringBuilder result = new StringBuilder();

                result.Append("<?xml version=\"1.0\" ?>");
                result.Append("<Neptuo-Web-Framework-Root");
                foreach (RegisteredNamespace entry in Registrator.GetRegisteredNamespaces())
                {
                    if (usedPrefixes.Add(entry.Prefix))
                        result.AppendFormat(" xmlns:{0}=\"clr-namespace:{1}\"", entry.Prefix, entry.Namespace);
                }
                result.Append(">");
                result.Append(content);
                result.Append("</Neptuo-Web-Framework-Root>");

                return result.ToString();
            }
        }
    }
}
