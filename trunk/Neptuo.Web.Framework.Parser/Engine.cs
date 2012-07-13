using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Parser
{
    public static class Engine
    {
        //public static List<IContentParser> ContentParsers { get; set; }

        //public static List<IValueContentParser> ValueParsers { get; set; }

        public static void ParseContent(string content)
        {
            //if (ContentParsers != null)
            //{
            //    foreach (IContentParser parser in ContentParsers)
            //    {
            //        if (parser.Parse(content))
            //            return;
            //    }
            //}
            throw new NotImplementedException();
        }

        public static void ParseValueContent(string content)
        {
            //if (ValueParsers != null)
            //{
            //    foreach (IValueContentParser parser in ValueParsers)
            //    {
            //        if (parser.Parse(content))
            //            return;
            //    }
            //}
            throw new NotImplementedException();
        }
    }
}
