//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18051
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SharpKit.JavaScript;
namespace Neptuo.Templates
{
    [JsType(JsMode.Clr, Filename = "res/View.js")]
    public sealed class View_38422005C8911AD1E3131BF96B087D39DBA789AA : Neptuo.Templates.BaseGeneratedView, System.IDisposable
    {
        private TestConsoleNG.Observers.DataContextObserver field2;
        protected override void CreateViewPageControls(Neptuo.Templates.IViewPage viewPage)
        {
            this.Content.Add("<!-- This is a html comment -->\r\n");
            this.Content.Add(this.field1_Create());
        }
        private TestConsoleNG.Controls.GenericContentControl field1_Create()
        {
            TestConsoleNG.Controls.GenericContentControl field1 = new TestConsoleNG.Controls.GenericContentControl(this.componentManager);
            this.componentManager.AddComponent(field1, this.field1_Bind);
            this.componentManager.AttachObserver(field1, this.field2_Create(), this.field2_Bind);
            return field1;
        }
        private TestConsoleNG.Observers.DataContextObserver field2_Create()
        {
            if ((this.field2 == null))
            {
                this.field2 = new TestConsoleNG.Observers.DataContextObserver(this.componentManager, ((TestConsoleNG.Data.DataStorage)(this.dependencyProvider.Resolve(typeof(TestConsoleNG.Data.DataStorage), null))));
                this.componentManager.AddComponent(field2, this.field2_Bind);
            }
            return this.field2;
        }
        private void field2_Bind(TestConsoleNG.Observers.DataContextObserver field2)
        {
            field2.DataContext = ((object)(this.field3_Create().ProvideValue(this.CreateValueExtensionContext(field2, "DataContext"))));
        }
        private TestConsoleNG.Extensions.BindingExtension field3_Create()
        {
            TestConsoleNG.Extensions.BindingExtension field3 = new TestConsoleNG.Extensions.BindingExtension(((TestConsoleNG.Data.DataStorage)(this.dependencyProvider.Resolve(typeof(TestConsoleNG.Data.DataStorage), null))), ((TestConsoleNG.Extensions.IValueConverterService)(this.dependencyProvider.Resolve(typeof(TestConsoleNG.Extensions.IValueConverterService), null))));
            field3.Expression = "Address";
            return field3;
        }
        private void field1_Bind(TestConsoleNG.Controls.GenericContentControl field1)
        {
            field1.TagName = "div";
            field1.Content = new System.Collections.Generic.List<object>();
            field1.Content.Add("\r\n    ");
            field1.Content.Add(this.field4_Create());
            field1.Content.Add("\r\n    ");
            field1.Content.Add(this.field9_Create());
            field1.Content.Add("\r\n    ");
            field1.Content.Add(this.field12_Create());
            field1.Content.Add("\r\n    ");
            field1.Content.Add(this.field15_Create());
            field1.Content.Add("\r\n");
        }
        private TestConsoleNG.Controls.PanelControl field4_Create()
        {
            TestConsoleNG.Controls.PanelControl field4 = new TestConsoleNG.Controls.PanelControl(this.componentManager);
            this.componentManager.AddComponent(field4, this.field4_Bind);
            this.componentManager.AttachObserver(field4, this.field5_Create(), this.field5_Bind);
            return field4;
        }
        private TestConsoleNG.Observers.VisibleObserver field5_Create()
        {
            TestConsoleNG.Observers.VisibleObserver field5 = new TestConsoleNG.Observers.VisibleObserver();
            this.componentManager.AddComponent(field5, this.field5_Bind);
            return field5;
        }
        private void field5_Bind(TestConsoleNG.Observers.VisibleObserver field5)
        {
            field5.Visible = this.CastValueTo<bool>(this.field6_Create().ProvideValue(this.CreateValueExtensionContext(field5, "Visible")));
        }
        private TestConsoleNG.Extensions.BindingExtension field6_Create()
        {
            TestConsoleNG.Extensions.BindingExtension field6 = new TestConsoleNG.Extensions.BindingExtension(((TestConsoleNG.Data.DataStorage)(this.dependencyProvider.Resolve(typeof(TestConsoleNG.Data.DataStorage), null))), ((TestConsoleNG.Extensions.IValueConverterService)(this.dependencyProvider.Resolve(typeof(TestConsoleNG.Extensions.IValueConverterService), null))));
            field6.ConverterKey = "NullToBool";
            field6.Expression = "Street";
            return field6;
        }
        private void field4_Bind(TestConsoleNG.Controls.PanelControl field4)
        {
            field4.Header = "Street:";
            field4.Content = new System.Collections.Generic.List<object>();
            field4.Content.Add("\r\n        ");
            field4.Content.Add(this.field7_Create());
            field4.Content.Add("\r\n    ");
        }
        private TestConsoleNG.Controls.TextBoxControl field7_Create()
        {
            TestConsoleNG.Controls.TextBoxControl field7 = new TestConsoleNG.Controls.TextBoxControl(this.componentManager);
            this.componentManager.AddComponent(field7, this.field7_Bind);
            return field7;
        }
        private void field7_Bind(TestConsoleNG.Controls.TextBoxControl field7)
        {
            field7.Name = "street";
            field7.Text = this.CastValueTo<string>(this.field8_Create().ProvideValue(this.CreateValueExtensionContext(field7, "Text")));
        }
        private TestConsoleNG.Extensions.BindingExtension field8_Create()
        {
            TestConsoleNG.Extensions.BindingExtension field8 = new TestConsoleNG.Extensions.BindingExtension(((TestConsoleNG.Data.DataStorage)(this.dependencyProvider.Resolve(typeof(TestConsoleNG.Data.DataStorage), null))), ((TestConsoleNG.Extensions.IValueConverterService)(this.dependencyProvider.Resolve(typeof(TestConsoleNG.Extensions.IValueConverterService), null))));
            field8.Expression = "Street";
            return field8;
        }
        private TestConsoleNG.Controls.PanelControl field9_Create()
        {
            TestConsoleNG.Controls.PanelControl field9 = new TestConsoleNG.Controls.PanelControl(this.componentManager);
            this.componentManager.AddComponent(field9, this.field9_Bind);
            return field9;
        }
        private void field9_Bind(TestConsoleNG.Controls.PanelControl field9)
        {
            field9.Header = "House Number:";
            field9.Content = new System.Collections.Generic.List<object>();
            field9.Content.Add("\r\n        ");
            field9.Content.Add(this.field10_Create());
            field9.Content.Add("\r\n    ");
        }
        private TestConsoleNG.Controls.TextBoxControl field10_Create()
        {
            TestConsoleNG.Controls.TextBoxControl field10 = new TestConsoleNG.Controls.TextBoxControl(this.componentManager);
            this.componentManager.AddComponent(field10, this.field10_Bind);
            return field10;
        }
        private void field10_Bind(TestConsoleNG.Controls.TextBoxControl field10)
        {
            field10.Name = "housenumber";
            field10.Text = this.CastValueTo<string>(this.field11_Create().ProvideValue(this.CreateValueExtensionContext(field10, "Text")));
        }
        private TestConsoleNG.Extensions.BindingExtension field11_Create()
        {
            TestConsoleNG.Extensions.BindingExtension field11 = new TestConsoleNG.Extensions.BindingExtension(((TestConsoleNG.Data.DataStorage)(this.dependencyProvider.Resolve(typeof(TestConsoleNG.Data.DataStorage), null))), ((TestConsoleNG.Extensions.IValueConverterService)(this.dependencyProvider.Resolve(typeof(TestConsoleNG.Extensions.IValueConverterService), null))));
            field11.Expression = "HouseNumber";
            return field11;
        }
        private TestConsoleNG.Controls.PanelControl field12_Create()
        {
            TestConsoleNG.Controls.PanelControl field12 = new TestConsoleNG.Controls.PanelControl(this.componentManager);
            this.componentManager.AddComponent(field12, this.field12_Bind);
            return field12;
        }
        private void field12_Bind(TestConsoleNG.Controls.PanelControl field12)
        {
            field12.Header = "City:";
            field12.Content = new System.Collections.Generic.List<object>();
            field12.Content.Add("\r\n        ");
            field12.Content.Add(this.field13_Create());
            field12.Content.Add("\r\n    ");
        }
        private TestConsoleNG.Controls.TextBoxControl field13_Create()
        {
            TestConsoleNG.Controls.TextBoxControl field13 = new TestConsoleNG.Controls.TextBoxControl(this.componentManager);
            this.componentManager.AddComponent(field13, this.field13_Bind);
            return field13;
        }
        private void field13_Bind(TestConsoleNG.Controls.TextBoxControl field13)
        {
            field13.Name = "city";
            field13.Text = this.CastValueTo<string>(this.field14_Create().ProvideValue(this.CreateValueExtensionContext(field13, "Text")));
        }
        private TestConsoleNG.Extensions.BindingExtension field14_Create()
        {
            TestConsoleNG.Extensions.BindingExtension field14 = new TestConsoleNG.Extensions.BindingExtension(((TestConsoleNG.Data.DataStorage)(this.dependencyProvider.Resolve(typeof(TestConsoleNG.Data.DataStorage), null))), ((TestConsoleNG.Extensions.IValueConverterService)(this.dependencyProvider.Resolve(typeof(TestConsoleNG.Extensions.IValueConverterService), null))));
            field14.Expression = "City";
            return field14;
        }
        private TestConsoleNG.Controls.PanelControl field15_Create()
        {
            TestConsoleNG.Controls.PanelControl field15 = new TestConsoleNG.Controls.PanelControl(this.componentManager);
            this.componentManager.AddComponent(field15, this.field15_Bind);
            return field15;
        }
        private void field15_Bind(TestConsoleNG.Controls.PanelControl field15)
        {
            field15.Header = "Postal code:";
            field15.Content = new System.Collections.Generic.List<object>();
            field15.Content.Add("\r\n        ");
            field15.Content.Add(this.field16_Create());
            field15.Content.Add("\r\n    ");
        }
        private TestConsoleNG.Controls.TextBoxControl field16_Create()
        {
            TestConsoleNG.Controls.TextBoxControl field16 = new TestConsoleNG.Controls.TextBoxControl(this.componentManager);
            this.componentManager.AddComponent(field16, this.field16_Bind);
            return field16;
        }
        private void field16_Bind(TestConsoleNG.Controls.TextBoxControl field16)
        {
            field16.Name = "postalcode";
            field16.Text = this.CastValueTo<string>(this.field17_Create().ProvideValue(this.CreateValueExtensionContext(field16, "Text")));
        }
        private TestConsoleNG.Extensions.BindingExtension field17_Create()
        {
            TestConsoleNG.Extensions.BindingExtension field17 = new TestConsoleNG.Extensions.BindingExtension(((TestConsoleNG.Data.DataStorage)(this.dependencyProvider.Resolve(typeof(TestConsoleNG.Data.DataStorage), null))), ((TestConsoleNG.Extensions.IValueConverterService)(this.dependencyProvider.Resolve(typeof(TestConsoleNG.Extensions.IValueConverterService), null))));
            field17.Expression = "PostalCode";
            return field17;
        }
    }
}
