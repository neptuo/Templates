/*Generated by SharpKit 5 v5.2.0*/
if (typeof($CreateException)=='undefined') 
{
    var $CreateException = function(ex, error) 
    {
        if(error==null)
            error = new Error();
        if(ex==null)
            ex = new System.Exception.ctor();       
        error.message = ex.message;
        for (var p in ex)
           error[p] = ex[p];
        return error;
    }
}
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
if (typeof ($CreateAnonymousDelegate) == 'undefined') {
    var $CreateAnonymousDelegate = function (target, func) {
        if (target == null || func == null)
            return func;
        var delegate = function () {
            return func.apply(target, arguments);
        };
        delegate.func = func;
        delegate.target = target;
        delegate.isDelegate = true;
        return delegate;
    }
}
if (typeof(JsTypes) == "undefined")
    var JsTypes = [];
var TestConsoleNG$Controls$AnchorControl =
{
    fullname: "TestConsoleNG.Controls.AnchorControl",
    baseTypeName: "TestConsoleNG.Controls.BaseContentControl",
    assemblyName: "TestConsoleNG.Components",
    customAttributes: [ {targetType: "type", typeName: "Neptuo.Templates.HtmlAttribute", ctorName: "ctor$$String", positionalArguments: ["a"]}],
    Kind: "Class",
    definition:
    {
        ctor: function (componentManager)
        {
            this._Url = null;
            this._Parameters = null;
            this._Security = null;
            TestConsoleNG.Controls.BaseContentControl.ctor.call(this, componentManager);
        },
        Url$$: "System.String",
        get_Url: function ()
        {
            return this._Url;
        },
        set_Url: function (value)
        {
            this._Url = value;
        },
        Parameters$$: "System.Collections.Generic.ICollection`1[[TestConsoleNG.Controls.Parameter]]",
        get_Parameters: function ()
        {
            return this._Parameters;
        },
        set_Parameters: function (value)
        {
            this._Parameters = value;
        },
        Security$$: "TestConsoleNG.Controls.Security",
        get_Security: function ()
        {
            return this._Security;
        },
        set_Security: function (value)
        {
            this._Security = value;
        },
        OnInit: function ()
        {
            TestConsoleNG.Controls.BaseContentControl.commonPrototype.OnInit.call(this);
            this.Init$$Object(this.get_Security());
            if (this.get_Security() != null)
            {
                this.get_Parameters().Add((function ()
                {
                    var $v1 = new TestConsoleNG.Controls.Parameter.ctor();
                    $v1.set_Name("Security");
                    $v1.set_Value(this.get_Security().get_Identifier());
                    return $v1;
                }).call(this));
            }
            this.Init$1$$IEnumerable$1(TestConsoleNG.Controls.Parameter.ctor, this.get_Parameters());
            var $it1 = this.get_Parameters().GetEnumerator();
            while ($it1.MoveNext())
            {
                var parameter = $it1.get_Current();
                if (this.get_Url().Contains("?"))
                    this.set_Url(this.get_Url() + "&");
                else
                    this.set_Url(this.get_Url() + "?");
                this.set_Url(this.get_Url() + System.String.Format$$String$$Object$$Object("{0}={1}", parameter.get_Name(), parameter.get_Value()));
            }
            this.get_Attributes().set_Item$$TKey("href", this.get_Url());
        }
    }
};
JsTypes.push(TestConsoleNG$Controls$AnchorControl);
var TestConsoleNG$Controls$Parameter =
{
    fullname: "TestConsoleNG.Controls.Parameter",
    baseTypeName: "System.Object",
    assemblyName: "TestConsoleNG.Components",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            this._Name = null;
            this._Value = null;
            System.Object.ctor.call(this);
        },
        Name$$: "System.String",
        get_Name: function ()
        {
            return this._Name;
        },
        set_Name: function (value)
        {
            this._Name = value;
        },
        Value$$: "System.String",
        get_Value: function ()
        {
            return this._Value;
        },
        set_Value: function (value)
        {
            this._Value = value;
        }
    }
};
JsTypes.push(TestConsoleNG$Controls$Parameter);
var TestConsoleNG$Controls$Security =
{
    fullname: "TestConsoleNG.Controls.Security",
    baseTypeName: "System.Object",
    assemblyName: "TestConsoleNG.Components",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            this._Identifier = null;
            System.Object.ctor.call(this);
        },
        Identifier$$: "System.String",
        get_Identifier: function ()
        {
            return this._Identifier;
        },
        set_Identifier: function (value)
        {
            this._Identifier = value;
        }
    }
};
JsTypes.push(TestConsoleNG$Controls$Security);
var TestConsoleNG$Controls$BaseContentControl =
{
    fullname: "TestConsoleNG.Controls.BaseContentControl",
    baseTypeName: "TestConsoleNG.Controls.BaseControl",
    assemblyName: "TestConsoleNG.Components",
    interfaceNames: ["Neptuo.Templates.Controls.IContentControl"],
    Kind: "Class",
    definition:
    {
        ctor: function (componentManager)
        {
            this._Content = null;
            TestConsoleNG.Controls.BaseControl.ctor.call(this, componentManager);
        },
        IsSelfClosing$$: "System.Boolean",
        get_IsSelfClosing: function ()
        {
            if (this.get_Content() != null && this.get_Content().get_Count() != 0)
                return false;
            return TestConsoleNG.Controls.BaseControl.commonPrototype.get_IsSelfClosing.call(this);
        },
        Content$$: "System.Collections.Generic.ICollection`1[[System.Object]]",
        get_Content: function ()
        {
            return this._Content;
        },
        set_Content: function (value)
        {
            this._Content = value;
        },
        OnInit: function ()
        {
            TestConsoleNG.Controls.BaseControl.commonPrototype.OnInit.call(this);
            if (this.get_Content() != null)
            {
                var $it2 = this.get_Content().GetEnumerator();
                while ($it2.MoveNext())
                {
                    var item = $it2.get_Current();
                    this.get_ComponentManager().Init(item);
                }
            }
        },
        RenderBody: function (writer)
        {
            if (this.get_Content() != null)
            {
                var $it3 = this.get_Content().GetEnumerator();
                while ($it3.MoveNext())
                {
                    var item = $it3.get_Current();
                    this.get_ComponentManager().Render(item, writer);
                }
            }
        }
    }
};
JsTypes.push(TestConsoleNG$Controls$BaseContentControl);
var TestConsoleNG$Controls$BaseControl =
{
    fullname: "TestConsoleNG.Controls.BaseControl",
    baseTypeName: "System.Object",
    assemblyName: "TestConsoleNG.Components",
    interfaceNames: ["Neptuo.Templates.Controls.IControl", "Neptuo.Templates.IAttributeCollection"],
    Kind: "Class",
    definition:
    {
        ctor: function (componentManager)
        {
            this.tagName = null;
            this.isSelfClosing = null;
            this.defaultPropertyName = null;
            this._Attributes = null;
            this._ComponentManager = null;
            System.Object.ctor.call(this);
            this.set_ComponentManager(componentManager);
            this.set_Attributes(new Neptuo.Templates.HtmlAttributeCollection.ctor());
        },
        Attributes$$: "Neptuo.Templates.HtmlAttributeCollection",
        get_Attributes: function ()
        {
            return this._Attributes;
        },
        set_Attributes: function (value)
        {
            this._Attributes = value;
        },
        ComponentManager$$: "Neptuo.Templates.IComponentManager",
        get_ComponentManager: function ()
        {
            return this._ComponentManager;
        },
        set_ComponentManager: function (value)
        {
            this._ComponentManager = value;
        },
        TagName$$: "System.String",
        get_TagName: function ()
        {
            if (this.tagName == null)
            {
                var attr = Neptuo.Reflection.ReflectionHelper.GetAttribute$1(Neptuo.Templates.HtmlAttribute.ctor, this.GetType());
                if (attr != null)
                    this.tagName = attr.get_TagName();
            }
            return this.tagName;
        },
        set_TagName: function (value)
        {
            this.tagName = value;
        },
        IsSelfClosing$$: "System.Boolean",
        get_IsSelfClosing: function ()
        {
            if (this.isSelfClosing == null)
            {
                var attr = Neptuo.Reflection.ReflectionHelper.GetAttribute$1(Neptuo.Templates.HtmlAttribute.ctor, this.GetType());
                if (attr != null)
                    this.isSelfClosing = attr.get_IsSelfClosing();
            }
            return (this.isSelfClosing != null ? this.isSelfClosing : true);
        },
        set_IsSelfClosing: function (value)
        {
            this.isSelfClosing = value;
        },
        DefaultPropertyName$$: "System.String",
        get_DefaultPropertyName: function ()
        {
            if (this.defaultPropertyName == null)
            {
                var attr = Neptuo.Reflection.ReflectionHelper.GetAttribute$1(System.ComponentModel.DefaultPropertyAttribute.ctor, this.GetType());
                if (attr != null)
                    this.defaultPropertyName = attr.get_Name();
            }
            return this.defaultPropertyName;
        },
        set_DefaultPropertyName: function (value)
        {
            this.defaultPropertyName = value;
        },
        OnInit: function ()
        {
        },
        Render: function (writer)
        {
            if (!System.String.IsNullOrEmpty(this.get_TagName()))
            {
                writer.Tag(this.get_TagName());
                var $it4 = this.get_Attributes().GetEnumerator();
                while ($it4.MoveNext())
                {
                    var attribute = $it4.get_Current();
                    writer.Attribute(attribute.get_Key(), attribute.get_Value());
                }
                if (this.get_IsSelfClosing())
                {
                    writer.CloseTag();
                }
                else
                {
                    this.RenderBody(writer);
                    writer.CloseFullTag();
                }
            }
            else
            {
                this.RenderBody(writer);
            }
        },
        RenderBody: function (writer)
        {
        },
        Init$$Object: function (component)
        {
            this.get_ComponentManager().Init(component);
        },
        Init$1$$IEnumerable$1: function (T, compoments)
        {
            if (compoments != null)
            {
                var $it5 = compoments.GetEnumerator();
                while ($it5.MoveNext())
                {
                    var component = $it5.get_Current();
                    this.Init$$Object(component);
                }
            }
        },
        SetAttribute: function (name, value)
        {
            this.get_Attributes().set_Item$$TKey(name, value);
        }
    }
};
JsTypes.push(TestConsoleNG$Controls$BaseControl);
var TestConsoleNG$Controls$GenericContentControl =
{
    fullname: "TestConsoleNG.Controls.GenericContentControl",
    baseTypeName: "TestConsoleNG.Controls.BaseContentControl",
    assemblyName: "TestConsoleNG.Components",
    Kind: "Class",
    definition:
    {
        ctor: function (componentManager)
        {
            TestConsoleNG.Controls.BaseContentControl.ctor.call(this, componentManager);
        },
        TagName$$: "System.String",
        get_TagName: function ()
        {
            return TestConsoleNG.Controls.BaseControl.commonPrototype.get_TagName.call(this);
        },
        set_TagName: function (value)
        {
            TestConsoleNG.Controls.BaseControl.commonPrototype.set_TagName.call(this, value);
        },
        Render: function (writer)
        {
            if (!System.String.IsNullOrEmpty(this.get_TagName()))
                TestConsoleNG.Controls.BaseControl.commonPrototype.Render.call(this, writer);
        }
    }
};
JsTypes.push(TestConsoleNG$Controls$GenericContentControl);
var TestConsoleNG$Controls$LiteralControl =
{
    fullname: "TestConsoleNG.Controls.LiteralControl",
    baseTypeName: "TestConsoleNG.Controls.BaseControl",
    assemblyName: "TestConsoleNG.Components",
    Kind: "Class",
    definition:
    {
        ctor: function (componentManager)
        {
            this._Text = null;
            TestConsoleNG.Controls.BaseControl.ctor.call(this, componentManager);
        },
        Text$$: "System.String",
        get_Text: function ()
        {
            return this._Text;
        },
        set_Text: function (value)
        {
            this._Text = value;
        },
        RenderBody: function (writer)
        {
            writer.Content$$String(this.get_Text());
        }
    }
};
JsTypes.push(TestConsoleNG$Controls$LiteralControl);
var TestConsoleNG$Controls$PanelControl =
{
    fullname: "TestConsoleNG.Controls.PanelControl",
    baseTypeName: "TestConsoleNG.Controls.BaseContentControl",
    assemblyName: "TestConsoleNG.Components",
    customAttributes: [ {targetType: "type", typeName: "Neptuo.Templates.HtmlAttribute", ctorName: "ctor$$String", positionalArguments: ["div"]}],
    Kind: "Class",
    definition:
    {
        ctor: function (componentManager)
        {
            this._Header = null;
            TestConsoleNG.Controls.BaseContentControl.ctor.call(this, componentManager);
        },
        Header$$: "System.String",
        get_Header: function ()
        {
            return this._Header;
        },
        set_Header: function (value)
        {
            this._Header = value;
        },
        RenderBody: function (writer)
        {
            if (!System.String.IsNullOrEmpty(this.get_Header()))
            {
                writer.Tag("div").Attribute("class", "panel-header").Content$$String(this.get_Header()).CloseTag();
            }
            TestConsoleNG.Controls.BaseContentControl.commonPrototype.RenderBody.call(this, writer);
        }
    }
};
JsTypes.push(TestConsoleNG$Controls$PanelControl);
var TestConsoleNG$Controls$TextBoxControl =
{
    fullname: "TestConsoleNG.Controls.TextBoxControl",
    baseTypeName: "TestConsoleNG.Controls.BaseControl",
    assemblyName: "TestConsoleNG.Components",
    customAttributes: [ {targetType: "type", typeName: "Neptuo.Templates.HtmlAttribute", ctorName: "ctor$$String$$Boolean", positionalArguments: ["input", true]}],
    Kind: "Class",
    definition:
    {
        ctor: function (componentManager)
        {
            TestConsoleNG.Controls.BaseControl.ctor.call(this, componentManager);
        },
        Name$$: "System.String",
        get_Name: function ()
        {
            return this.get_Attributes().get_Item$$TKey("name");
        },
        set_Name: function (value)
        {
            this.get_Attributes().set_Item$$TKey("name", value);
        },
        Text$$: "System.String",
        get_Text: function ()
        {
            return this.get_Attributes().get_Item$$TKey("value");
        },
        set_Text: function (value)
        {
            this.get_Attributes().set_Item$$TKey("value", value);
        },
        OnInit: function ()
        {
            TestConsoleNG.Controls.BaseControl.commonPrototype.OnInit.call(this);
            this.get_Attributes().set_Item$$TKey("type", "text");
        },
        Render: function (writer)
        {
            TestConsoleNG.Controls.BaseControl.commonPrototype.Render.call(this, writer);
        }
    }
};
JsTypes.push(TestConsoleNG$Controls$TextBoxControl);
var TestConsoleNG$Data$DataStorage =
{
    fullname: "TestConsoleNG.Data.DataStorage",
    baseTypeName: "System.Object",
    assemblyName: "TestConsoleNG.Components",
    Kind: "Class",
    definition:
    {
        ctor: function (data)
        {
            this.storage = new System.Collections.Generic.Stack$1.ctor(System.Object.ctor);
            System.Object.ctor.call(this);
            this.Push(data);
        },
        Push: function (data)
        {
            this.storage.Push(data);
        },
        Pop: function ()
        {
            return this.storage.Pop();
        },
        Peek: function ()
        {
            return this.storage.Peek();
        }
    }
};
JsTypes.push(TestConsoleNG$Data$DataStorage);
var TestConsoleNG$Extensions$BindingExtension =
{
    fullname: "TestConsoleNG.Extensions.BindingExtension",
    baseTypeName: "System.Object",
    assemblyName: "TestConsoleNG.Components",
    interfaceNames: ["Neptuo.Templates.Extensions.IValueExtension"],
    Kind: "Class",
    definition:
    {
        ctor: function (dataStorage, valueConverterService)
        {
            this.dataStorage = null;
            this.valueConverterService = null;
            this._Expression = null;
            this._ConverterKey = null;
            System.Object.ctor.call(this);
            this.dataStorage = dataStorage;
            this.valueConverterService = valueConverterService;
        },
        Expression$$: "System.String",
        get_Expression: function ()
        {
            return this._Expression;
        },
        set_Expression: function (value)
        {
            this._Expression = value;
        },
        ConverterKey$$: "System.String",
        get_ConverterKey: function ()
        {
            return this._ConverterKey;
        },
        set_ConverterKey: function (value)
        {
            this._ConverterKey = value;
        },
        ProvideValue: function (context)
        {
            var data = TestConsoleNG.Extensions.BindingManager.GetValue(this.get_Expression(), this.dataStorage.Peek());
            if (!System.String.IsNullOrEmpty(this.get_ConverterKey()))
                data = this.valueConverterService.GetConverter(this.get_ConverterKey()).ConvertTo(data);
            return data;
        }
    }
};
JsTypes.push(TestConsoleNG$Extensions$BindingExtension);
var TestConsoleNG$Extensions$BindingManager =
{
    fullname: "TestConsoleNG.Extensions.BindingManager",
    baseTypeName: "System.Object",
    staticDefinition:
    {
        SetValue: function (target, expression, value)
        {
            var info = null;
            var type = target.GetType();
            var exprs = expression.Split$$Char$Array$$StringSplitOptions(["."], 1);
            for (var i = 0; i < exprs.length; i++)
            {
                info = type.GetProperty$$String(exprs[i]);
                type = info.get_PropertyType();
                if (i < (exprs.length - 1))
                    target = info.GetValue$$Object$$Object$Array(target, null);
            }
            if (System.Reflection.PropertyInfo.op_Inequality$$PropertyInfo$$PropertyInfo(info, null))
                info.SetValue$$Object$$Object$$Object$Array(target, value, null);
        },
        GetValue: function (expression, value)
        {
            if (System.String.IsNullOrEmpty(expression))
                return value;
            if (value == null)
                return null;
            var info = null;
            var type = value.GetType();
            var exprs = expression.Split$$Char$Array$$StringSplitOptions(["."], 1);
            for (var i = 0; i < exprs.length; i++)
            {
                info = type.GetProperty$$String(exprs[i]);
                if (System.Reflection.PropertyInfo.op_Equality$$PropertyInfo$$PropertyInfo(info, null))
                    return null;
                type = info.get_PropertyType();
                if (value != null)
                    value = info.GetValue$$Object$$Object$Array(value, null);
            }
            return value;
        }
    },
    assemblyName: "TestConsoleNG.Components",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            System.Object.ctor.call(this);
        }
    }
};
JsTypes.push(TestConsoleNG$Extensions$BindingManager);
var TestConsoleNG$Extensions$IValueConverter = {fullname: "TestConsoleNG.Extensions.IValueConverter", baseTypeName: "System.Object", assemblyName: "TestConsoleNG.Components", Kind: "Interface"};
JsTypes.push(TestConsoleNG$Extensions$IValueConverter);
var TestConsoleNG$Extensions$IValueConverterService = {fullname: "TestConsoleNG.Extensions.IValueConverterService", baseTypeName: "System.Object", assemblyName: "TestConsoleNG.Components", Kind: "Interface"};
JsTypes.push(TestConsoleNG$Extensions$IValueConverterService);
var TestConsoleNG$Extensions$NullToBoolValueConverter =
{
    fullname: "TestConsoleNG.Extensions.NullToBoolValueConverter",
    baseTypeName: "System.Object",
    assemblyName: "TestConsoleNG.Components",
    interfaceNames: ["TestConsoleNG.Extensions.IValueConverter"],
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            System.Object.ctor.call(this);
        },
        ConvertTo: function (value)
        {
            return value != null;
        }
    }
};
JsTypes.push(TestConsoleNG$Extensions$NullToBoolValueConverter);
var TestConsoleNG$Extensions$ValueConverterService =
{
    fullname: "TestConsoleNG.Extensions.ValueConverterService",
    baseTypeName: "System.Object",
    assemblyName: "TestConsoleNG.Components",
    interfaceNames: ["TestConsoleNG.Extensions.IValueConverterService"],
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            this.storage = new System.Collections.Generic.Dictionary$2.ctor(System.String.ctor, TestConsoleNG.Extensions.IValueConverter.ctor);
            System.Object.ctor.call(this);
        },
        GetConverter: function (key)
        {
            if (System.String.IsNullOrEmpty(key))
                throw $CreateException(new System.ArgumentNullException.ctor$$String$$String("key", "Key can\'t be null or empty!"), new Error());
            if (!this.storage.ContainsKey(key))
                throw $CreateException(new System.ArgumentOutOfRangeException.ctor$$String$$Object$$String("key", "There is no converter associated with key {0}!", key), new Error());
            return this.storage.get_Item$$TKey(key);
        },
        SetConverter: function (key, converter)
        {
            this.storage.set_Item$$TKey(key, converter);
            return this;
        }
    }
};
JsTypes.push(TestConsoleNG$Extensions$ValueConverterService);
var TestConsoleNG$Observers$DataContextObserver =
{
    fullname: "TestConsoleNG.Observers.DataContextObserver",
    baseTypeName: "System.Object",
    assemblyName: "TestConsoleNG.Components",
    interfaceNames: ["Neptuo.Templates.Observers.IObserver"],
    Kind: "Class",
    definition:
    {
        ctor: function (componentManager, dataStorage)
        {
            this.componentManager = null;
            this.dataStorage = null;
            this._DataContext = null;
            System.Object.ctor.call(this);
            this.componentManager = componentManager;
            this.dataStorage = dataStorage;
        },
        DataContext$$: "System.Object",
        get_DataContext: function ()
        {
            return this._DataContext;
        },
        set_DataContext: function (value)
        {
            this._DataContext = value;
        },
        OnInit: function (e)
        {
            this.componentManager.AttachInitComplete(e.get_Target(), $CreateDelegate(this, this.OnInitComplete));
            this.dataStorage.Push(this.get_DataContext());
        },
        OnInitComplete: function (e)
        {
            this.dataStorage.Pop();
        },
        Render: function (e, writer)
        {
        }
    }
};
JsTypes.push(TestConsoleNG$Observers$DataContextObserver);
var TestConsoleNG$Observers$VisibleObserver =
{
    fullname: "TestConsoleNG.Observers.VisibleObserver",
    baseTypeName: "System.Object",
    assemblyName: "TestConsoleNG.Components",
    interfaceNames: ["Neptuo.Templates.Observers.IObserver"],
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            this._Visible = false;
            System.Object.ctor.call(this);
        },
        Visible$$: "System.Boolean",
        get_Visible: function ()
        {
            return this._Visible;
        },
        set_Visible: function (value)
        {
            this._Visible = value;
        },
        OnInit: function (e)
        {
            if (!this.get_Visible())
                e.set_Cancel(true);
        },
        Render: function (e, writer)
        {
            if (!this.get_Visible())
                e.set_Cancel(true);
        }
    }
};
JsTypes.push(TestConsoleNG$Observers$VisibleObserver);
var TestConsoleNG$PersonModel =
{
    fullname: "TestConsoleNG.PersonModel",
    baseTypeName: "System.Object",
    assemblyName: "TestConsoleNG.Components",
    Kind: "Class",
    definition:
    {
        ctor: function (firstname, lastname, address)
        {
            this._Firstname = null;
            this._Lastname = null;
            this._Address = null;
            System.Object.ctor.call(this);
            this.set_Firstname(firstname);
            this.set_Lastname(lastname);
            this.set_Address(address);
        },
        Firstname$$: "System.String",
        get_Firstname: function ()
        {
            return this._Firstname;
        },
        set_Firstname: function (value)
        {
            this._Firstname = value;
        },
        Lastname$$: "System.String",
        get_Lastname: function ()
        {
            return this._Lastname;
        },
        set_Lastname: function (value)
        {
            this._Lastname = value;
        },
        Address$$: "TestConsoleNG.AddressModel",
        get_Address: function ()
        {
            return this._Address;
        },
        set_Address: function (value)
        {
            this._Address = value;
        }
    }
};
JsTypes.push(TestConsoleNG$PersonModel);
var TestConsoleNG$AddressModel =
{
    fullname: "TestConsoleNG.AddressModel",
    baseTypeName: "System.Object",
    assemblyName: "TestConsoleNG.Components",
    Kind: "Class",
    definition:
    {
        ctor: function (street, houseNumber, city, postalCode)
        {
            this._Street = null;
            this._HouseNumber = 0;
            this._City = null;
            this._PostalCode = 0;
            System.Object.ctor.call(this);
            this.set_Street(street);
            this.set_HouseNumber(houseNumber);
            this.set_City(city);
            this.set_PostalCode(postalCode);
        },
        Street$$: "System.String",
        get_Street: function ()
        {
            return this._Street;
        },
        set_Street: function (value)
        {
            this._Street = value;
        },
        HouseNumber$$: "System.Int32",
        get_HouseNumber: function ()
        {
            return this._HouseNumber;
        },
        set_HouseNumber: function (value)
        {
            this._HouseNumber = value;
        },
        City$$: "System.String",
        get_City: function ()
        {
            return this._City;
        },
        set_City: function (value)
        {
            this._City = value;
        },
        PostalCode$$: "System.Int32",
        get_PostalCode: function ()
        {
            return this._PostalCode;
        },
        set_PostalCode: function (value)
        {
            this._PostalCode = value;
        }
    }
};
JsTypes.push(TestConsoleNG$AddressModel);
var TestConsoleNG$SimpleContainer$SimpleObjectBuilder =
{
    fullname: "TestConsoleNG.SimpleContainer.SimpleObjectBuilder",
    baseTypeName: "System.Object",
    assemblyName: "TestConsoleNG.Components",
    interfaceNames: ["Neptuo.IDependencyContainer"],
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            this.registry = null;
            TestConsoleNG.SimpleContainer.SimpleObjectBuilder.ctor$$Dictionary$2.call(this, new System.Collections.Generic.Dictionary$2.ctor(System.Type.ctor, System.Func$1.ctor));
        },
        ctor$$Dictionary$2: function (registry)
        {
            this.registry = null;
            System.Object.ctor.call(this);
            this.registry = registry;
        },
        RegisterInstance: function (t, name, instance)
        {
            this.registry.set_Item$$TKey(t, $CreateAnonymousDelegate(this, function ()
            {
                return instance;
            }));
            return this;
        },
        RegisterType: function (from, to, name)
        {
            this.registry.set_Item$$TKey(from, $CreateAnonymousDelegate(this, function ()
            {
                return System.Activator.CreateInstance$$Type(to);
            }));
            return this;
        },
        CreateChildContainer: function ()
        {
            return new TestConsoleNG.SimpleContainer.SimpleObjectBuilder.ctor$$Dictionary$2(new System.Collections.Generic.Dictionary$2.ctor$$IDictionary$2(System.Type.ctor, System.Func$1.ctor, this.registry));
        },
        Resolve: function (t, name)
        {
            if (this.registry.ContainsKey(t))
                return this.registry.get_Item$$TKey(t)();
            return System.Activator.CreateInstance$$Type(t);
        },
        ResolveAll: function (t)
        {
            throw $CreateException(new System.NotImplementedException.ctor(), new Error());
        }
    }
};
JsTypes.push(TestConsoleNG$SimpleContainer$SimpleObjectBuilder);
