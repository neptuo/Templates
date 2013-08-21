using SharpKit.JavaScript;
using SharpKit.Html;
using SharpKit.jQuery;
using Neptuo.Templates;
using System.IO;

namespace TestSharpKit
{
    [JsType(JsMode.Global, Filename = "res/Default.js")]
    public class DefaultClient
    {
        static void DefaultClient_Load()
        {
            new jQuery(HtmlContext.document.body).append("Ready<br/>");
        }
        static void btnTest_click(DOMEvent e)
        {
            var componentManager = new ComponentManager();

            var view = new View_C612CAF502A06BAAC6171B58D1EA2F61EC9D6D55();
            view.Setup(new BaseViewPage(componentManager), componentManager, null);
            view.CreateControls();
            view.Init();
            view.Render(new HtmlTextWriter(new StringWriter()));
            view.Dispose();
        }
    }
}