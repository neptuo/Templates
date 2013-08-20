using SharpKit.JavaScript;
using SharpKit.Html;
using SharpKit.jQuery;
using Neptuo.Templates;

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

            //var view = new View_B7C1FA09BECBCD1D79251994E26F3C4BB8C31E11();
            //view.Setup(new BaseViewPage(componentManager), componentManager, null);
            //view.CreateControls();
            //view.Init();
            //view.Render(new HtmlTextWriter(null));
            //view.Dispose();
        }
    }
}