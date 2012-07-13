using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Parser.HtmlContent;
using Neptuo.Web.Framework.Controls;
using Neptuo.Web.Framework.Utils;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Neptuo.Web.Framework.Compilation
{
    public class ControlContentCompiler : IContentCompiler<HtmlTag>
    {
        public void GenerateCode(HtmlTag parsedItem, ContentCompilerContext context)
        {
            CodeGenerator generator = context.CodeGenerator;
            Type controlType = ((IRegistrator)context.ServiceProvider.GetService(typeof(IRegistrator))).GetControl(parsedItem.Namespace, parsedItem.Name);

            if (controlType != null)
            {
                CodeMemberField control = generator.DeclareField(controlType);
                generator.CreateInstance(control, typeof(LiteralControl));

                BindProperties(control, controlType, parsedItem, context);

                generator.AddToViewPage(control);
            }
            else
            {
                //TODO: Handle, exception? Some fallback?
                AppendPlainText(parsedItem.Fullname, context);
            }
        }

        public void AppendPlainText(string text, ContentCompilerContext context)
        {
            CodeGenerator generator = context.CodeGenerator;

            CodeMemberField declareText = generator.DeclareField(typeof(LiteralControl));
            generator.CreateInstance(declareText, typeof(LiteralControl));
            generator.SetProperty(declareText, "Text", text);
            generator.AddToViewPage(declareText);
        }

        private void BindProperties(CodeMemberField control, Type controlType, HtmlTag parsedItem, ContentCompilerContext context)
        {
            CodeGenerator generator = context.CodeGenerator;
            HashSet<string> boundProperies = new HashSet<string>();

            foreach (KeyValuePair<string, PropertyInfo> item in ControlHelper.GetProperties(controlType))
            {
                bool bound = false;
                foreach (HtmlAttribute attribute in parsedItem.Attributes)
                {
                    if (item.Key.ToLowerInvariant() == attribute.Name.ToLowerInvariant())
                    {
                        //TODO: Convert values ...
                        generator.SetProperty(control, item.Key, attribute.Value);
                        boundProperies.Add(item.Key);
                        bound = true;
                    }
                }

                if (!bound && !String.IsNullOrEmpty(parsedItem.Content))
                {
                    // Najdi to, co může být property v obsahu
                    HtmlContentParser parser = new HtmlContentParser(new HtmlContentParser.Configuration
                    {
                         StartTagRegex = new Regex(String.Format("<(?<TagName>{0})", item.Key))
                    });
                    parser.OnParsedItem += (e) =>
                    {
                        string startSubstring = e.OriginalContent.Substring(0, e.StartPosition);
                        if (String.IsNullOrEmpty(startSubstring) || String.IsNullOrWhiteSpace(startSubstring))
                        {
                            //TODO: Convert values ...
                            //TODO: Parse inner controls!
                            boundProperies.Add(item.Key);
                            generator.SetProperty(control, item.Key, e.ParsedItem.Content);
                            bound = true;
                        }
                    };
                    parser.Parse(parsedItem.Content);
                }
            }

            //PropertyInfo defaultProperty = ControlHelper.GetDefaultProperty(controlType);
            //if(defaultProperty != null)
        }
    }
}
