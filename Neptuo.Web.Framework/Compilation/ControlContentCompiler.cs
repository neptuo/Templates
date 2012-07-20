using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Neptuo.Web.Framework.Controls;
using Neptuo.Web.Framework.Parser;
using Neptuo.Web.Framework.Parser.HtmlContent;
using Neptuo.Web.Framework.Utils;
using System.ComponentModel;
using TypeConverter = Neptuo.Web.Framework.Utils.TypeConverter;
using Neptuo.Web.Framework.Annotations;

namespace Neptuo.Web.Framework.Compilation
{
    public class ControlContentCompiler : IContentCompiler<HtmlTag>
    {
        public void GenerateCode(HtmlTag parsedItem, ContentCompilerContext context)
        {
            Type controlType = ((IRegistrator)context.ServiceProvider.GetService(typeof(IRegistrator))).GetControl(parsedItem.Namespace, parsedItem.Name);

            if (controlType != null)
            {
                ControlBuilderAttribute attr = ControlBuilderAttribute.GetAttribute(controlType);
                if (attr != null)
                {
                    IControlBuilder builder = Activator.CreateInstance(attr.BuilderType) as IControlBuilder;
                    if (builder != null)
                    {
                        builder.GenerateControl(controlType, parsedItem, new ControlBuilderContext(context, this));
                        return;
                    }
                }

                GenerateControl(controlType, parsedItem, context);
            }
            else
            {
                //TODO: Handle, exception? Some fallback?
                AppendPlainText(parsedItem.Fullname, context);
            }
        }

        public void AppendPlainText(string text, ContentCompilerContext context)
        {
            bool canUse = context.ParentInfo.RequiredType.IsAssignableFrom(typeof(LiteralControl));

            if (!canUse)
                return;

            CodeGenerator generator = context.CodeGenerator;

            CodeMemberField declareText = generator.DeclareField(typeof(LiteralControl));
            generator.CreateInstance(declareText, typeof(LiteralControl));
            generator.SetProperty(declareText, "Text", text);
            generator.AddToParent(context.ParentInfo, declareText);
        }

        public void GenerateControl(Type controlType, HtmlTag parsedItem, ContentCompilerContext context)
        {
            CodeGenerator generator = context.CodeGenerator;

            CodeMemberField control = generator.DeclareField(controlType);
            generator.CreateInstance(control, controlType);
            generator.AddToParent(context.ParentInfo, control);

            BindProperties(control, controlType, parsedItem, context);
        }

        private void BindProperties(CodeMemberField control, Type controlType, HtmlTag parsedItem, ContentCompilerContext context)
        {
            CodeGenerator generator = context.CodeGenerator;
            HashSet<string> boundProperies = new HashSet<string>();
            Dictionary<string, ContentParserEventArgs<HtmlTag>> innerTags = ParseInnerTags(parsedItem);
            PropertyInfo defaultProperty = ControlHelper.GetDefaultProperty(controlType);

            int lastInnerTagStart = 0;
            foreach (KeyValuePair<string, PropertyInfo> item in ControlHelper.GetProperties(controlType))
            {
                bool bound = false;
                string propertyName = item.Key.ToLowerInvariant();
                foreach (HtmlAttribute attribute in parsedItem.Attributes)
                {
                    if (propertyName == attribute.Name.ToLowerInvariant())
                    {
                        if (TypeConverter.CanConvert(item.Value.PropertyType))
                            generator.SetProperty(control, item.Key, TypeConverter.Convert(attribute.Value, item.Value.PropertyType));
                        else
                            BindPropertyDefaultValue(control, context, item.Value);

                        boundProperies.Add(propertyName);
                        bound = true;
                    }
                }

                if (!bound && innerTags.ContainsKey(propertyName))
                {
                    var args = innerTags[propertyName];
                    string startSubstring = parsedItem.Content.Substring(lastInnerTagStart, args.StartPosition - lastInnerTagStart);
                    if (args.StartPosition != lastInnerTagStart && !(String.IsNullOrEmpty(startSubstring) || String.IsNullOrWhiteSpace(startSubstring)))
                        throw new ApplicationException("Error in format. Properties set as body elements must appear at the body beginning.");

                    lastInnerTagStart = args.EndPosition;

                    ResolvePropertyValue(control, controlType, context, item.Value, args.ParsedItem.Content);
                    boundProperies.Add(propertyName);
                    bound = true;
                }

                if (!bound && item.Value != defaultProperty)
                {
                    BindPropertyDefaultValue(control, context, item.Value);
                    boundProperies.Add(propertyName);
                    bound = true;
                }
            }

            if (defaultProperty != null)
            {
                if (parsedItem.Content != null && lastInnerTagStart < parsedItem.Content.Length)
                    ResolvePropertyValue(control, controlType, context, defaultProperty, parsedItem.Content.Substring(lastInnerTagStart));
                else
                    BindPropertyDefaultValue(control, context, defaultProperty);
            }
        }

        private void BindPropertyDefaultValue(CodeMemberField control, ContentCompilerContext context, PropertyInfo prop)
        {
            DefaultValueAttribute attr = ReflectionHelper.GetAttribute<DefaultValueAttribute>(prop);
            if (attr != null)
                context.CodeGenerator.SetProperty(control, prop.Name, attr.Value);
        }

        private Dictionary<string, ContentParserEventArgs<HtmlTag>> ParseInnerTags(HtmlTag tag)
        {
            Dictionary<string, ContentParserEventArgs<HtmlTag>> result = new Dictionary<string, ContentParserEventArgs<HtmlTag>>();

            HtmlContentParser parser = new HtmlContentParser(new HtmlContentParser.Configuration
            {
                //StartTagRegex = new Regex(@"<(?<TagName>\w+)")
                StartTagRegex = new Regex(@"<(?<TagName>\w+)( |>)")
            });
            parser.OnParsedItem += (e) =>
            {
                result.Add(e.ParsedItem.Fullname.ToLowerInvariant(), e);
            };
            parser.Parse(tag.Content);

            return result;
        }

        private void ResolvePropertyValue(CodeMemberField control, Type controlType, ContentCompilerContext context, PropertyInfo prop, string value)
        {
            CodeGenerator generator = context.CodeGenerator;
            if (ReflectionHelper.IsGenericType<IList>(prop.PropertyType))
            {
                ParentInfo parent = context.ParentInfo;
                context.ParentInfo = new ParentInfo(control.Name, prop.Name, "Add", ReflectionHelper.GetGenericArgument(prop.PropertyType));

                RunParser(context, value);

                context.ParentInfo = parent;
            }
            else if (TypeConverter.CanConvert(prop.PropertyType))
            {
                generator.SetProperty(control, prop.Name, TypeConverter.Convert(value, prop.PropertyType));
            }
            else
            {
                ParentInfo parent = context.ParentInfo;
                context.ParentInfo = new ParentInfo(control.Name, prop.Name, null, ReflectionHelper.GetGenericArgument(prop.PropertyType));

                RunParser(context, value);

                context.ParentInfo = parent;
            }
        }

        private void RunParser(ContentCompilerContext context, string value)
        {
            int lastEndPosition = 0;

            var parser = (context.Parser as IContentParser<HtmlTag>);
            var onParsedItem = parser.OnParsedItem;
            var onParserDone = parser.OnParserDone;

            parser.OnParsedItem = e =>
            {
                if (e.StartPosition > lastEndPosition)
                    AppendPlainText(e.OriginalContent.Substring(lastEndPosition, e.StartPosition - lastEndPosition), context);

                lastEndPosition = e.EndPosition;
                GenerateCode(e.ParsedItem, context);
            };
            parser.OnParserDone = e =>
            {
                if (lastEndPosition < e.OriginalContent.Length)
                    AppendPlainText(e.OriginalContent.Substring(lastEndPosition), context);

            };

            parser.Parse(value);

            parser.OnParsedItem = onParsedItem;
            parser.OnParserDone = onParserDone;
        }
    }
}
