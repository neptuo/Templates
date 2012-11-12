using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoWebUI.Models
{
    public class LiveModel
    {
        public string SessionID { get; set; }
        [AllowHtml]
        public string ViewContent { get; set; }
    }
}