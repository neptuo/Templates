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
            StringWriter writer = new StringWriter();

            var componentManager = new ComponentManager();
            var view = new View_38422005C8911AD1E3131BF96B087D39DBA789AA();
            view.Setup(new BaseViewPage(componentManager), componentManager, null);
            view.CreateControls();
            view.Init();
            view.Render(new HtmlTextWriter(writer));
            view.Dispose();

            HtmlContext.alert(writer.ToString());
        }
    }
}