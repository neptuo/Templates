using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates
{
    public class HtmlTextWriterException : Exception
    {
        public HtmlTextWriterException(string message)
            : base(message)
        { }
    }
}
