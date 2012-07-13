using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Parser.HtmlContent
{
    public class StateMachineResult
    {
        public HtmlTag HtmlTag { get; set; }

        public int LastIndex { get; set; }
    }
}
