using SharpKit.JavaScript;
using SharpKit.Html;
using SharpKit.jQuery;
using Neptuo.Templates;
using System.IO;
using MagicWare.ObjectBuilder;
using TestConsoleNG.Data;
using TestConsoleNG;
using TestConsoleNG.Extensions;

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
            JavascriptFactory container = new JavascriptFactory();
            container.RegisterInstance<DataStorage>(new DataStorage(new PersonModel("Jon", "Doe", new AddressModel("Dlouhá street", 23, "Prague", 10001))));
            container.RegisterInstance<IValueConverterService>(new ValueConverterService().SetConverter("NullToBool", new NullToBoolValueConverter()));
            container.RegisterInstance<IComponentManager>(new ComponentManager());


            StringWriter writer = new StringWriter();

            var view = new View_38422005C8911AD1E3131BF96B087D39DBA789AA();
            view.Setup(new BaseViewPage(container.Resolve<IComponentManager>()), container.Resolve<IComponentManager>(), container);
            view.CreateControls();
            view.Init();
            view.Render(new HtmlTextWriter(writer));
            view.Dispose();

            HtmlContext.alert(writer.ToString());
        }
    }
}