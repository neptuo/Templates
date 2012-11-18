using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public interface IErrorInfo
    {
        int Line { get; }
        int Column { get; }
        string ErrorNumber { get; }
        string ErrorText { get; }
    }
}
