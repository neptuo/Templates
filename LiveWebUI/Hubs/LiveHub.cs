using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LiveWebUI.Hubs
{
    public class LiveHub : Hub
    {
        public string DefaultView()
        {
            return File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/Index.html"));
        }

        public string GetSourceCode(string sourceCode)
        {
            return sourceCode;
        }
    }
}