using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public class ErrorInfo : IErrorInfo
    {
        public int Line { get; set; }
        public int Column { get; set; }
        public string ErrorNumber { get; set; }
        public string ErrorText { get; set; }

        public ErrorInfo()
        { }

        public ErrorInfo(string errorText)
        {
            ErrorText = errorText;
        }

        public ErrorInfo(int line, int column, string errorText)
        {
            Line = line;
            Column = column;
            ErrorText = errorText;
        }
    }
}
