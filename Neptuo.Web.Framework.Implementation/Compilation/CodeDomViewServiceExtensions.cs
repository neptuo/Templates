using Neptuo.Web.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    public static class CodeDomViewServiceExtensions
    {
        public static void LoadSection(this CodeDomViewService viewService, string name = "neptuo.web.framework/codeDom")
        {
            CodeDomSection section = (CodeDomSection)ConfigurationManager.GetSection(name);
            viewService.DebugMode = section.DebugMode;
            viewService.TempDirectory = section.TempDirectory;

            foreach (PathElement element in section.BinDirectories)
                viewService.BinDirectories.Add(element.Path);
        }
    }
}
