using Neptuo;
using Neptuo.Activators;
using SharpKit.Html;
using SharpKit.JavaScript;
using SharpKit.jQuery;
using System;
using System.IO;
using Test.Templates;
using Test.Templates.Runtime;
using Test.Templates.UI.Converters;
using Test.Templates.UI.Data;
using Test.Templates.UI.Models;

namespace Test.Templates.SharpKit.WebSite
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
            throw new NotImplementedException();
            IDependencyContainer container = null;
            //container.RegisterInstance<DataStorage>(new DataStorage(new PersonModel("Jon", "Doe", new AddressModel("Dlouhá street", 23, "Prague", 10001))));
            //container.RegisterInstance<IValueConverterService>(new ValueConverterService().SetConverter("NullToBool", new NullToBoolValueConverter()));
            //container.RegisterInstance<IComponentManager>(new ComponentManager());

            StringWriter writer = new StringWriter();

            //var view = new View_38422005C8911AD1E3131BF96B087D39DBA789AA();
            //view.Setup(container);
            //view.OnInit(container.Resolve<IComponentManager>());
            //view.Render(new HtmlTextWriter(writer));
            //view.Dispose();

            new jQuery("#viewContent").html(writer.ToString());
        }
    }
}