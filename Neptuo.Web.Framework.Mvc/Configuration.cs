using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Html;

namespace Neptuo.Web.Mvc.ViewEngine
{
    public class Configuration
    {
        public Dictionary<string, List<string>> Namespaces { get; protected set; }
        public Dictionary<string, Dictionary<string, Type>> TypesInNamespaces { get; protected set; }

        public Dictionary<string, List<IHandler>> Handlers { get; protected set; }
        public Dictionary<string, List<IAttributeHandler>> AttributeHandlers { get; protected set; }

        public Configuration()
        {
            Namespaces = new Dictionary<string, List<string>>();
            TypesInNamespaces = new Dictionary<string, Dictionary<string, Type>>();
            Handlers = new Dictionary<string, List<IHandler>>();
            AttributeHandlers = new Dictionary<string, List<IAttributeHandler>>();
        }
    }

    //public class 
}
