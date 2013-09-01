using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiveWebUI.Hubs
{
    public interface IViewRepository
    {
        void SetContent(string identifier, string viewContent);

        string GetContent(string identifier);
    }
}