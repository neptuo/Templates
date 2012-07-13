using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Parser.HtmlContent
{
    public class HtmlTag
    {
        public string Namespace { get; set; }

        public string Name { get; set; }

        public string Fullname
        {
            get
            {
                if (String.IsNullOrEmpty(Namespace))
                    return Name;

                return Namespace + ":" + Name;
            }
            set
            {
                int index = value.IndexOf(':');
                if (index != -1)
                {
                    Namespace = value.Substring(0, index);
                    Name = value.Substring(index + 1);
                }
                else
                {
                    Name = value;
                }
            }
        }

        public List<HtmlAttribute> Attributes { get; set; }

        public string Content { get; set; }

        public HtmlTag()
        {
            Attributes = new List<HtmlAttribute>();
        }
    }
}
