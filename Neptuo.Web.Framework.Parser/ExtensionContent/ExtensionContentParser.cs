using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Parser.ExtensionContent
{
    public class ExtensionContentParser : IValueContentParser<Extension>
    {
        public OnValueParsedItem<Extension> OnParsedItem { get; set; }

        public bool Parse(string content)
        {
            if (OnParsedItem == null)
                throw new ArgumentNullException("OnParsedItem");

            if (content.StartsWith("{"))
            {
                try
                {
                    StateMachine machine = new StateMachine();
                    StateMachineResult result = machine.Process(content);

                    if (result.Extension == null)
                        return false;

                    ValueParserEventArgs<Extension> args = new ValueParserEventArgs<Extension>();
                    args.ParsedItem = result.Extension;
                    args.OriginalContent = content;
                    OnParsedItem(args);

                    return true;
                }
                catch (StateMachineException)
                {
                    return false;
                }
            }

            return false;
        }
    }

    public delegate void OnExtension(ExtensionEventArgs args);
}
