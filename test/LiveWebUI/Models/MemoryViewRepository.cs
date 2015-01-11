using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiveWebUI.Models
{
    public class MemoryViewRepository : IViewRepository
    {
        Dictionary<string, string> views = new Dictionary<string, string>();

        public string GetContent(string identifier)
        {
            if (views.ContainsKey(identifier))
                return views[identifier];

            return null;
        }

        public void SetContent(string identifier, string viewContent)
        {
            views[identifier] = viewContent;
        }
    }
}