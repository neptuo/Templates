using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Neptuo.Web.Framework.Annotations;
using Neptuo.Web.Framework.Utils;
using TypeConverter = Neptuo.Web.Framework.Utils.TypeConverter;

namespace Neptuo.Web.Framework.Compilation
{
    public class DefaultContentGenerator : IContentCodeGenerator
    {
        private Dictionary<Type, CodeObjectCreator> globalObservers = new Dictionary<Type, CodeObjectCreator>();
        private Dictionary<object, Dictionary<Type, CodeObjectCreator>> controlObservers = new Dictionary<object, Dictionary<Type, CodeObjectCreator>>();
        private Type literalType;
        private string literalTextPropertyName;
        private Type genericContentType;
        private string genericContentTagNamePropertyName;

        public DefaultContentGenerator(Type literalType, string literalTextPropertyName, Type genericContentType, string genericContentTagNamePropertyName)
        {
            this.literalType = literalType;
            this.literalTextPropertyName = literalTextPropertyName;
            this.genericContentType = genericContentType;
            this.genericContentTagNamePropertyName = genericContentTagNamePropertyName;
        }

        public bool GenerateCode(string content, ContentGeneratorContext context)
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

                        Type controlType;
                        if (String.IsNullOrWhiteSpace(element.Prefix))
                            controlType = genericContentType;
                        else
                            controlType = helper.Registrator.GetControl(element.Prefix, element.LocalName);

                        if (controlType != null)
                        {
                            BuilderAttribute attr = BuilderAttribute.GetAttribute(controlType);
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
            if (element == null)
                return false;

            if(!String.IsNullOrWhiteSpace(element.Prefix))
                return true;

            foreach (XmlAttribute attribute in element.Attributes)
            {
                if (!String.IsNullOrWhiteSpace(attribute.Prefix))
                    return true;
            }

            return false;
        }

        private void AppendPlainText(string text, ContentGeneratorContext context)
        {
            bool canUse = context.ParentInfo.RequiredType.IsAssignableFrom(literalType);

            if (!canUse)
                return;

            if (String.IsNullOrWhiteSpace(text))
                return;

            text = text.Trim();

            context.CodeGenerator.CreateControl()
                .Declare(literalType)
                .CreateInstance()
                .SetProperty(literalTextPropertyName, text)
                .AddToParent(context.ParentInfo)
                .RegisterLivecycleObserver(context.ParentInfo);
        }

        public void GenerateControl(Helper helper, Type controlType, XmlElement element)
        {
            CodeObjectCreator creator = helper.Context.CodeGenerator
                .CreateControl()
                .Declare(controlType)
                .CreateInstance()
                .RegisterLivecycleObserver(helper.Context.ParentInfo);

            if (String.IsNullOrWhiteSpace(element.Prefix))
                creator.SetProperty(genericContentTagNamePropertyName, element.Name);

            BindProperties(helper, creator, element);

            creator
                .AddToParent(helper.Context.ParentInfo);
        }

        private void BindProperties(Helper helper, CodeObjectCreator creator, XmlElement element)
        {
            HashSet<string> boundProperies = new HashSet<string>();
            PropertyInfo defaultProperty = ControlHelper.GetDefaultProperty(creator.FieldType);
            List<XmlAttribute> observerAttributes = new List<XmlAttribute>();

            foreach (KeyValuePair<string, PropertyInfo> item in ControlHelper.GetProperties(creator.FieldType))
            {
                bool bound = false;
                string propertyName = item.Key.ToLowerInvariant();
                foreach (XmlAttribute attribute in element.Attributes)
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

                    if (!bound && !String.IsNullOrWhiteSpace(attribute.Prefix) && !observerAttributes.Contains(attribute))
                        observerAttributes.Add(attribute);
                }

                XmlNode child;
                if (!bound && FindChildNode(element, propertyName, out child))
                {
                    ResolvePropertyValue(helper, creator, item.Value, child.ChildNodes.ToEnumerable());
                    //ResolvePropertyValue(control, controlType, context, item.Value, args.ParsedItem.Content);
                    boundProperies.Add(propertyName);
                    bound = true;
                }

                if (!bound && item.Value != defaultProperty)
                {
                    BindPropertyDefaultValue(helper, creator, item.Value);
                    boundProperies.Add(propertyName);
                    bound = true;
                }
            }

            foreach (XmlAttribute attribute in observerAttributes)
            {
                Type observerType = helper.Registrator.GetObserver(attribute.Prefix, attribute.LocalName);
                if (observerType != null)
                {
                    //TODO: Register observer
                    RegisterObserver(helper, creator, observerType, attribute);
                }
            }

            if (defaultProperty != null && !boundProperies.Contains(defaultProperty.Name.ToLowerInvariant()))
            {
                IEnumerable<XmlNode> defaultChildNodes = FindNotUsedChildNodes(element, boundProperies);
                if (defaultChildNodes.Any())
                    ResolvePropertyValue(helper, creator, defaultProperty, defaultChildNodes);
                else
                    BindPropertyDefaultValue(helper, creator, defaultProperty);
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

        private void ResolvePropertyValue(Helper helper, CodeObjectCreator creator, PropertyInfo prop, IEnumerable<XmlNode> content)
        {
            if (ReflectionHelper.IsGenericType<IList>(prop.PropertyType))
            {
                ParentInfo parent = helper.Context.ParentInfo;
                helper.Context.ParentInfo = new ParentInfo(
                    creator, 
                    prop.Name, 
                    TypeHelper.MethodName<IList, object, int>(l => l.Add), 
                    ReflectionHelper.GetGenericArgument(prop.PropertyType)
                );

                GenerateRecursive(helper, content);

                helper.Context.ParentInfo = parent;
            }
            else if (TypeConverter.CanConvert(prop.PropertyType))
            {
                StringBuilder contentValue = new StringBuilder();
                foreach (XmlNode node in content)
                    contentValue.Append(node.OuterXml);

                creator.SetProperty(prop.Name, TypeConverter.Convert(contentValue.ToString(), prop.PropertyType));
            }
            else
            {
                ParentInfo parent = helper.Context.ParentInfo;
                helper.Context.ParentInfo = new ParentInfo(creator, prop.Name, null, prop.PropertyType);

                GenerateRecursive(helper, content);

                helper.Context.ParentInfo = parent;
            }
        }

        private void RegisterObserver(Helper helper, CodeObjectCreator creator, Type observerType, XmlAttribute attribute)
        {
            ObserverLivecycle livecycle = ObserverLivecycle.PerAttribute;

            ObserverAttribute observerAttribute = ObserverAttribute.GetAttribute(observerType);
            if (observerAttribute != null)
                livecycle = observerAttribute.Livecycle;

            CodeObjectCreator observer = helper.Context.CodeGenerator.CreateControl();
            if (livecycle == ObserverLivecycle.PerPage && globalObservers.ContainsKey(observerType))
            {
                //TODO: Nutno vyřešit problém se jménem bind metody
                //observer.Field = globalObservers[observerType].Field;
                //observer.FieldType = globalObservers[observerType].FieldType;
                observer = globalObservers[observerType];
            }
            else if (livecycle == ObserverLivecycle.PerControl && controlObservers.ContainsKey(creator) && controlObservers[creator].ContainsKey(observerType))
            {
                //TODO: Nutno vyřešit problém se jménem bind metody
                //observer.Field = controlObservers[creator][observerType].Field;
                //observer.FieldType = controlObservers[creator][observerType].FieldType;
                //observer.CreateBindMethod(typeof(object));
                observer = controlObservers[creator][observerType];
            }
            else
            {
                //TODO: ObserverBuilder
                BuilderAttribute builder = BuilderAttribute.GetAttribute(observerType);
                if (builder != null)
                {
                    throw new NotImplementedException("Remove parent info before supporting ObserverBuilder");
                }
                else
                {
                    observer = helper.Context.CodeGenerator
                        .CreateControl()
                        .Declare(observerType, typeof(object))
                        .CreateInstance();
                }

                if (livecycle == ObserverLivecycle.PerPage)
                {
                    globalObservers.Add(observerType, observer);
                }
                else if (livecycle == ObserverLivecycle.PerControl)
                {
                    if (!controlObservers.ContainsKey(creator))
                        controlObservers.Add(creator, new Dictionary<Type, CodeObjectCreator>());

                    controlObservers[creator].Add(observerType, observer);
                }
            }

            ParentInfo parent = helper.Context.ParentInfo;
            helper.Context.ParentInfo = new ParentInfo(observer, null, null, typeof(object)) { AsReturnStatement = true };

            helper.Context.GeneratorService.ProcessValue(attribute.Value, helper.Context);

            helper.Context.ParentInfo = parent;

            creator.RegisterObserver(attribute.LocalName, observer);
        }

        public class Helper
        {
            public ContentGeneratorContext Context { get; protected set; }

            public XmlDocument Document { get; protected set; }

            public IRegistrator Registrator { get; protected set; }

            public StringBuilder Content { get; protected set; }

            public Helper(string xml, ContentGeneratorContext context)
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
