using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Neptuo.Web.Framework.Parser.HtmlContent
{
    [Obsolete]
    public class HtmlContentParser : IContentParser<HtmlTag>
    {
        private static readonly Regex StartTagRegex = new Regex(@"<(?<TagNamespace>\w+):(?<TagName>\w+)");


        public OnParsedItem<HtmlTag> OnParsedItem { get; set; }

        public OnParserDone OnParserDone { get; set; }

        public Configuration Config { get; protected set; }

        public HtmlContentParser(Configuration config = null)
        {
            if (config == null)
                config = new Configuration();

            Config = new Configuration
            {
                StartTagRegex = config.StartTagRegex ?? StartTagRegex
            };
        }

        public bool Parse(string content)
        {
            if (OnParsedItem == null)
                throw new ArgumentNullException("OnHtmlTag");

            if (String.IsNullOrEmpty(content))
                return true;

            int lastIndex = -1;
            MatchCollection results = Config.StartTagRegex.Matches(content);
            foreach (Match match in results)
            {
                try
                {
                    if (lastIndex != -1 && match.Index < lastIndex)
                        continue;

                    StateMachine machine = new StateMachine();
                    StateMachineResult machineResult = machine.Process(content.Substring(match.Index), match.Groups["TagNamespace"].Value, match.Groups["TagName"].Value);
                    int parsedLastIndex = match.Index + machineResult.LastIndex;

                    ContentParserEventArgs<HtmlTag> args = new ContentParserEventArgs<HtmlTag>();
                    args.ParsedItem = machineResult.HtmlTag;
                    args.StartPosition = match.Index;
                    args.EndPosition = parsedLastIndex;
                    args.OriginalContent = content;
                    
                    OnParsedItem(args);

                    if (parsedLastIndex > lastIndex)
                        lastIndex = parsedLastIndex;
                }
                catch (StateMachineException)
                {
                    continue;
                }
            }

            if (OnParserDone != null)
            {
                OnParserDone(new ContentParserDoneEventArgs
                {
                    OriginalContent = content
                });
            }

            return true;
        }


        public class Configuration
        {
            public Regex StartTagRegex { get; set; }
        }
    }
}
