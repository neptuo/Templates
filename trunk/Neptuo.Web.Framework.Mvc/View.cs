using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Neptuo.Web.Framework.Compilation;

namespace Neptuo.Web.Framework.Mvc
{
    /// <summary>
    /// Html view.
    /// </summary>
    public class View : IView
    {
        private StandartCodeCompiler compiler;

        public string ViewName { get; protected set; }

        public string MasterName { get; protected set; }

        public bool UseCache { get; protected set; }

        public View(StandartCodeCompiler compiler, string viewName, string masterName = null, bool? useCache = null)
        {
            this.compiler = compiler;
            ViewName = viewName;
            MasterName = masterName;
            UseCache = useCache ?? true;
        }

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            string result = compiler.ProcessView(HttpContext.Current.Server.MapPath(ViewName));
            writer.Write(result);
        }
    }
}
