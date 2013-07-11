using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Parser.ExtensionContent
{
    public class Extension
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

        public List<ExtensionAttribute> Attributes { get; set; }

        public string DefaultAttributeValue { get; set; }

        public Extension()
        {
            Attributes = new List<ExtensionAttribute>();
        }
    }
}
