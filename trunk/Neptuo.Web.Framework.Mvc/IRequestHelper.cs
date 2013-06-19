using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Mvc
{
    public interface IRequestHelper
    {
        string ApplicationPath { get; }
        string AppRelativeCurrentExecutionFilePath { get; }
    }
}
