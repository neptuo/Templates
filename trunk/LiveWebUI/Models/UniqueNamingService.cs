using Neptuo.Templates.Compilation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiveWebUI.Models
{
    public class UniqueNamingService : NamingServiceBase
    {
        protected override string GetNameForContent(string viewContent)
        {
            return String.Format(ViewNameFormat, DateTime.Now.Ticks);
        }

        protected override string GetNameForFile(string fileName)
        {
            return String.Format(ViewNameFormat, DateTime.Now.Ticks);
        }
    }
}