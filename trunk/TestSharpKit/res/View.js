/*Generated by SharpKit 5 v5.2.0*/
if (typeof($CreateDelegate)=='undefined'){
    if(typeof($iKey)=='undefined') var $iKey = 0;
    if(typeof($pKey)=='undefined') var $pKey = String.fromCharCode(1);
    var $CreateDelegate = function(target, func){
        if (target == null || func == null) 
            return func;
        if(func.target==target && func.func==func)
            return func;
        if (target.$delegateCache == null)
            target.$delegateCache = {};
        if (func.$key == null)
            func.$key = $pKey + String(++$iKey);
        var delegate;
        if(target.$delegateCache!=null)
            delegate = target.$delegateCache[func.$key];
        if (delegate == null){
            delegate = function(){
                return func.apply(target, arguments);
            };
            delegate.func = func;
            delegate.target = target;
            delegate.isDelegate = true;
            if(target.$delegateCache!=null)
                target.$delegateCache[func.$key] = delegate;
        }
        return delegate;
    }
}
if (typeof(JsTypes) == "undefined")
    var JsTypes = [];
var Neptuo$Templates$View_9741084CDDEB89E6278CC0A58C5F284F84983F2D =
{
    fullname: "Neptuo.Templates.View_9741084CDDEB89E6278CC0A58C5F284F84983F2D",
    baseTypeName: "Neptuo.Templates.BaseGeneratedView",
    assemblyName: "TestSharpKit",
    interfaceNames: ["System.IDisposable"],
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            Neptuo.Templates.BaseGeneratedView.ctor.call(this);
        },
        CreateViewPageControls: function (viewPage)
        {
            this.get_Content().Add("<div>");
            this.get_Content().Add(this.field1_Create());
            this.get_Content().Add(this.field4_Create());
            this.get_Content().Add(this.field7_Create());
            this.get_Content().Add(this.field10_Create());
            this.get_Content().Add("</div>");
        },
        field1_Create: function ()
        {
            var field1 = new TestConsoleNG.Controls.PanelControl.ctor(this.componentManager);
            this.componentManager.AddComponent$1(TestConsoleNG.Controls.PanelControl.ctor, field1, $CreateDelegate(this, this.field1_Bind));
            return field1;
        },
        field1_Bind: function (field1)
        {
            field1.set_Content(new System.Collections.Generic.List$1.ctor(System.Object.ctor));
            field1.get_Content().Add(" \r\n        Street: \r\n        ");
            field1.get_Content().Add(this.field2_Create());
        },
        field2_Create: function ()
        {
            var field2 = new TestConsoleNG.Controls.TextBoxControl.ctor(this.componentManager);
            this.componentManager.AddComponent$1(TestConsoleNG.Controls.TextBoxControl.ctor, field2, $CreateDelegate(this, this.field2_Bind));
            return field2;
        },
        field2_Bind: function (field2)
        {
            field2.set_Name("street");
            field2.set_Text((Cast((this.field3_Create().ProvideValue(new Neptuo.Templates.Extensions.DefaultMarkupExtensionContext.ctor(field2, field2.GetType().GetProperty$$String("Text"), this.dependencyProvider))), System.String.ctor)));
        },
        field3_Create: function ()
        {
            var field3 = new TestConsoleNG.Extensions.BindingExtension.ctor((Cast((this.dependencyProvider.Resolve(Typeof(TestConsoleNG.Data.DataStorage.ctor), null)), TestConsoleNG.Data.DataStorage.ctor)));
            field3.set_Expression("Street");
            return field3;
        },
        field4_Create: function ()
        {
            var field4 = new TestConsoleNG.Controls.PanelControl.ctor(this.componentManager);
            this.componentManager.AddComponent$1(TestConsoleNG.Controls.PanelControl.ctor, field4, $CreateDelegate(this, this.field4_Bind));
            return field4;
        },
        field4_Bind: function (field4)
        {
            field4.set_Content(new System.Collections.Generic.List$1.ctor(System.Object.ctor));
            field4.get_Content().Add("\r\n        House Number:\r\n        ");
            field4.get_Content().Add(this.field5_Create());
        },
        field5_Create: function ()
        {
            var field5 = new TestConsoleNG.Controls.TextBoxControl.ctor(this.componentManager);
            this.componentManager.AddComponent$1(TestConsoleNG.Controls.TextBoxControl.ctor, field5, $CreateDelegate(this, this.field5_Bind));
            return field5;
        },
        field5_Bind: function (field5)
        {
            field5.set_Name("housenumber");
            field5.set_Text((Cast((this.field6_Create().ProvideValue(new Neptuo.Templates.Extensions.DefaultMarkupExtensionContext.ctor(field5, field5.GetType().GetProperty$$String("Text"), this.dependencyProvider))), System.String.ctor)));
        },
        field6_Create: function ()
        {
            var field6 = new TestConsoleNG.Extensions.BindingExtension.ctor((Cast((this.dependencyProvider.Resolve(Typeof(TestConsoleNG.Data.DataStorage.ctor), null)), TestConsoleNG.Data.DataStorage.ctor)));
            field6.set_Expression("HouseNumber");
            return field6;
        },
        field7_Create: function ()
        {
            var field7 = new TestConsoleNG.Controls.PanelControl.ctor(this.componentManager);
            this.componentManager.AddComponent$1(TestConsoleNG.Controls.PanelControl.ctor, field7, $CreateDelegate(this, this.field7_Bind));
            return field7;
        },
        field7_Bind: function (field7)
        {
            field7.set_Content(new System.Collections.Generic.List$1.ctor(System.Object.ctor));
            field7.get_Content().Add(" \r\n        City: \r\n        ");
            field7.get_Content().Add(this.field8_Create());
        },
        field8_Create: function ()
        {
            var field8 = new TestConsoleNG.Controls.TextBoxControl.ctor(this.componentManager);
            this.componentManager.AddComponent$1(TestConsoleNG.Controls.TextBoxControl.ctor, field8, $CreateDelegate(this, this.field8_Bind));
            return field8;
        },
        field8_Bind: function (field8)
        {
            field8.set_Name("city");
            field8.set_Text((Cast((this.field9_Create().ProvideValue(new Neptuo.Templates.Extensions.DefaultMarkupExtensionContext.ctor(field8, field8.GetType().GetProperty$$String("Text"), this.dependencyProvider))), System.String.ctor)));
        },
        field9_Create: function ()
        {
            var field9 = new TestConsoleNG.Extensions.BindingExtension.ctor((Cast((this.dependencyProvider.Resolve(Typeof(TestConsoleNG.Data.DataStorage.ctor), null)), TestConsoleNG.Data.DataStorage.ctor)));
            field9.set_Expression("City");
            return field9;
        },
        field10_Create: function ()
        {
            var field10 = new TestConsoleNG.Controls.PanelControl.ctor(this.componentManager);
            this.componentManager.AddComponent$1(TestConsoleNG.Controls.PanelControl.ctor, field10, $CreateDelegate(this, this.field10_Bind));
            return field10;
        },
        field10_Bind: function (field10)
        {
            field10.set_Content(new System.Collections.Generic.List$1.ctor(System.Object.ctor));
            field10.get_Content().Add("\r\n        Postal code:\r\n        ");
            field10.get_Content().Add(this.field11_Create());
        },
        field11_Create: function ()
        {
            var field11 = new TestConsoleNG.Controls.TextBoxControl.ctor(this.componentManager);
            this.componentManager.AddComponent$1(TestConsoleNG.Controls.TextBoxControl.ctor, field11, $CreateDelegate(this, this.field11_Bind));
            return field11;
        },
        field11_Bind: function (field11)
        {
            field11.set_Name("postalcode");
            field11.set_Text((Cast((this.field12_Create().ProvideValue(new Neptuo.Templates.Extensions.DefaultMarkupExtensionContext.ctor(field11, field11.GetType().GetProperty$$String("Text"), this.dependencyProvider))), System.String.ctor)));
        },
        field12_Create: function ()
        {
            var field12 = new TestConsoleNG.Extensions.BindingExtension.ctor((Cast((this.dependencyProvider.Resolve(Typeof(TestConsoleNG.Data.DataStorage.ctor), null)), TestConsoleNG.Data.DataStorage.ctor)));
            field12.set_Expression("PostalCode");
            return field12;
        }
    }
};
JsTypes.push(Neptuo$Templates$View_9741084CDDEB89E6278CC0A58C5F284F84983F2D);
var Neptuo$Templates$View_C612CAF502A06BAAC6171B58D1EA2F61EC9D6D55 =
{
    fullname: "Neptuo.Templates.View_C612CAF502A06BAAC6171B58D1EA2F61EC9D6D55",
    baseTypeName: "Neptuo.Templates.BaseGeneratedView",
    assemblyName: "TestSharpKit",
    interfaceNames: ["System.IDisposable"],
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            Neptuo.Templates.BaseGeneratedView.ctor.call(this);
        },
        CreateViewPageControls: function (viewPage)
        {
            this.get_Content().Add("<div><h:panel> \r\n        Street: \r\n        <h:textbox name=\"street\" />");
            this.get_Content().Add("</h:panel>");
            this.get_Content().Add("</div>");
        }
    }
};
JsTypes.push(Neptuo$Templates$View_C612CAF502A06BAAC6171B58D1EA2F61EC9D6D55);
