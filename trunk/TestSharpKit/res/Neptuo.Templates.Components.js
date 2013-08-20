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
var Neptuo$Templates$HtmlAttributeCollection =
{
    fullname: "Neptuo.Templates.HtmlAttributeCollection",
    baseTypeName: "System.Collections.Generic.Dictionary$2",
    assemblyName: "Neptuo.Templates.Components",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            System.Collections.Generic.Dictionary$2.ctor.call(this, System.String.ctor, System.String.ctor);
        }
    }
};
JsTypes.push(Neptuo$Templates$HtmlAttributeCollection);
var Neptuo$Templates$BaseGeneratedView =
{
    fullname: "Neptuo.Templates.BaseGeneratedView",
    baseTypeName: "System.Object",
    assemblyName: "Neptuo.Templates.Components",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            this.viewPage = null;
            this.componentManager = null;
            this.dependencyProvider = null;
            System.Object.ctor.call(this);
        },
        Content$$: "System.Collections.Generic.ICollection`1[[System.Object]]",
        get_Content: function ()
        {
            return this.viewPage.get_Content();
        },
        Setup: function (viewPage, componentManager, dependencyProvider)
        {
            this.viewPage = viewPage;
            this.componentManager = componentManager;
            this.dependencyProvider = dependencyProvider;
        },
        CreateControls: function ()
        {
            this.componentManager.AddComponent$1(Neptuo.Templates.IViewPage.ctor, this.viewPage, $CreateDelegate(this, this.CreateViewPageControls));
        },
        Init: function ()
        {
            this.componentManager.Init(this.viewPage);
        },
        Render: function (writer)
        {
            this.viewPage.Render(writer);
        },
        Dispose: function ()
        {
            this.viewPage.Dispose();
        }
    }
};
JsTypes.push(Neptuo$Templates$BaseGeneratedView);
var Neptuo$Templates$BaseViewPage =
{
    fullname: "Neptuo.Templates.BaseViewPage",
    baseTypeName: "System.Object",
    assemblyName: "Neptuo.Templates.Components",
    interfaceNames: ["Neptuo.Templates.IViewPage"],
    Kind: "Class",
    definition:
    {
        ctor: function (componentManager)
        {
            this._ComponentManager = null;
            this._Content = null;
            System.Object.ctor.call(this);
            this.set_ComponentManager(componentManager);
            this.set_Content(new System.Collections.Generic.List$1.ctor(System.Object.ctor));
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
            var $it1 = this.get_Content().GetEnumerator();
            while ($it1.MoveNext())
            {
                var item = $it1.get_Current();
                this.get_ComponentManager().Init(item);
            }
        },
        Render: function (writer)
        {
            var $it2 = this.get_Content().GetEnumerator();
            while ($it2.MoveNext())
            {
                var item = $it2.get_Current();
                this.get_ComponentManager().Render(item, writer);
            }
        },
        Dispose: function ()
        {
            var $it3 = this.get_Content().GetEnumerator();
            while ($it3.MoveNext())
            {
                var item = $it3.get_Current();
                this.get_ComponentManager().Dispose(item);
            }
        }
    }
};
JsTypes.push(Neptuo$Templates$BaseViewPage);
var Neptuo$Templates$ComponentManager =
{
    fullname: "Neptuo.Templates.ComponentManager",
    baseTypeName: "System.Object",
    assemblyName: "Neptuo.Templates.Components",
    interfaceNames: ["Neptuo.Templates.IComponentManager"],
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            this.entries = new System.Collections.Generic.Dictionary$2.ctor(System.Object.ctor, Neptuo.Templates.ComponentManager.BaseComponentEntry.ctor);
            System.Object.ctor.call(this);
        },
        AddComponent$1: function (T, component, propertyBinder)
        {
            var entry = (function ()
            {
                var $v1 = new Neptuo.Templates.ComponentManager.ComponentEntry$1.ctor(T);
                $v1.set_Control(component);
                $v1.set_ArePropertiesBound(propertyBinder == null);
                $v1.set_PropertyBinder(propertyBinder);
                return $v1;
            }).call(this);
            this.entries.Add(component, entry);
        },
        AttachObserver$1: function (T, control, observer, propertyBinder)
        {
            if (!this.entries.ContainsKey(control))
                return;
            this.entries.get_Item$$TKey(control).get_Observers().Add(new Neptuo.Templates.ComponentManager.ObserverInfo$1.ctor(T, observer, propertyBinder));
        },
        AttachInitComplete: function (control, handler)
        {
            if (this.entries.ContainsKey(control))
                this.entries.get_Item$$TKey(control).get_InitComplete().Add(handler);
        },
        Init: function (control)
        {
            if (control == null || !this.entries.ContainsKey(control))
                return;
            var entry = this.entries.get_Item$$TKey(control);
            if (entry.get_IsInited())
                return;
            if (entry.get_IsDisposed())
                return;
            if (!entry.get_ArePropertiesBound())
                entry.BindProperties();
            entry.set_IsInited(true);
            var target = As(entry.get_Control(), Neptuo.Templates.Controls.IControl.ctor);
            if (target == null)
                return;
            var canInit = true;
            if (entry.get_Observers().get_Count() > 0)
            {
                var args = new Neptuo.Templates.Observers.ObserverEventArgs.ctor(target);
                var $it4 = entry.get_Observers().GetEnumerator();
                while ($it4.MoveNext())
                {
                    var info = $it4.get_Current();
                    if (!info.get_ArePropertiesBound())
                    {
                        info.BindProperties();
                        info.set_ArePropertiesBound(true);
                    }
                    info.get_Observer().OnInit(args);
                    if (args.get_Cancel())
                        canInit = false;
                }
            }
            if (canInit)
                target.OnInit();
            if (System.Linq.Enumerable.Any$1$$IEnumerable$1(Neptuo.Templates.OnInitComplete.ctor, entry.get_InitComplete()))
            {
                var args = new Neptuo.Templates.ControlInitCompleteEventArgs.ctor(target);
                var $it5 = entry.get_InitComplete().GetEnumerator();
                while ($it5.MoveNext())
                {
                    var handler = $it5.get_Current();
                    handler(args);
                }
            }
        },
        Render: function (control, writer)
        {
            if (control == null)
                return;
            if (control.GetType() == Typeof(System.String.ctor))
                writer.Write$$Object(control);
            if (!this.entries.ContainsKey(control))
                return;
            var entry = this.entries.get_Item$$TKey(control);
            if (!entry.get_IsInited())
                this.Init(control);
            if (entry.get_IsDisposed())
                return;
            var target = As(entry.get_Control(), Neptuo.Templates.Controls.IControl.ctor);
            if (target == null)
                return;
            var canRender = true;
            if (entry.get_Observers().get_Count() > 0)
            {
                var args = new Neptuo.Templates.Observers.ObserverEventArgs.ctor(target);
                var $it6 = entry.get_Observers().GetEnumerator();
                while ($it6.MoveNext())
                {
                    var info = $it6.get_Current();
                    if (!info.get_ArePropertiesBound())
                    {
                        info.BindProperties();
                        info.set_ArePropertiesBound(true);
                    }
                    info.get_Observer().Render(args, writer);
                    if (args.get_Cancel())
                        canRender = false;
                }
            }
            if (canRender)
                target.Render(writer);
        },
        Dispose: function (component)
        {
            if (!this.entries.ContainsKey(component))
                return;
            var entry = this.entries.get_Item$$TKey(component);
            if (!entry.get_IsInited())
                this.Init(component);
            if (entry.get_IsDisposed())
                return;
            var target = As(entry.get_Control(), System.IDisposable.ctor);
            if (target != null)
            {
                target.Dispose();
                entry.set_IsDisposed(true);
            }
        }
    }
};
JsTypes.push(Neptuo$Templates$ComponentManager);
var Neptuo$Templates$ComponentManager$BaseComponentEntry =
{
    fullname: "Neptuo.Templates.ComponentManager.BaseComponentEntry",
    baseTypeName: "System.Object",
    assemblyName: "Neptuo.Templates.Components",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            this._Control = null;
            this._PropertyBinder = null;
            this._Observers = null;
            this._InitComplete = null;
            this._ArePropertiesBound = false;
            this._IsInited = false;
            this._IsDisposed = false;
            System.Object.ctor.call(this);
            this.set_Observers(new System.Collections.Generic.List$1.ctor(Neptuo.Templates.ComponentManager.ObserverInfo.ctor));
            this.set_InitComplete(new System.Collections.Generic.List$1.ctor(Neptuo.Templates.OnInitComplete.ctor));
        },
        Control$$: "System.Object",
        get_Control: function ()
        {
            return this._Control;
        },
        set_Control: function (value)
        {
            this._Control = value;
        },
        PropertyBinder$$: "System.Delegate",
        get_PropertyBinder: function ()
        {
            return this._PropertyBinder;
        },
        set_PropertyBinder: function (value)
        {
            this._PropertyBinder = value;
        },
        Observers$$: "System.Collections.Generic.List`1[[Neptuo.Templates.ComponentManager+ObserverInfo]]",
        get_Observers: function ()
        {
            return this._Observers;
        },
        set_Observers: function (value)
        {
            this._Observers = value;
        },
        InitComplete$$: "System.Collections.Generic.List`1[[Neptuo.Templates.OnInitComplete]]",
        get_InitComplete: function ()
        {
            return this._InitComplete;
        },
        set_InitComplete: function (value)
        {
            this._InitComplete = value;
        },
        ArePropertiesBound$$: "System.Boolean",
        get_ArePropertiesBound: function ()
        {
            return this._ArePropertiesBound;
        },
        set_ArePropertiesBound: function (value)
        {
            this._ArePropertiesBound = value;
        },
        IsInited$$: "System.Boolean",
        get_IsInited: function ()
        {
            return this._IsInited;
        },
        set_IsInited: function (value)
        {
            this._IsInited = value;
        },
        IsDisposed$$: "System.Boolean",
        get_IsDisposed: function ()
        {
            return this._IsDisposed;
        },
        set_IsDisposed: function (value)
        {
            this._IsDisposed = value;
        }
    }
};
JsTypes.push(Neptuo$Templates$ComponentManager$BaseComponentEntry);
var Neptuo$Templates$ComponentManager$ComponentEntry$1 =
{
    fullname: "Neptuo.Templates.ComponentManager.ComponentEntry$1",
    baseTypeName: "Neptuo.Templates.ComponentManager.BaseComponentEntry",
    assemblyName: "Neptuo.Templates.Components",
    Kind: "Class",
    definition:
    {
        ctor: function (T)
        {
            this.T = T;
            this.control = null;
            this.propertyBinder = null;
            Neptuo.Templates.ComponentManager.BaseComponentEntry.ctor.call(this);
        },
        Control$$: "System.Object",
        get_Control: function ()
        {
            return this.control;
        },
        set_Control: function (value)
        {
            this.control = Cast(value, this.T);
        },
        PropertyBinder$$: "System.Delegate",
        get_PropertyBinder: function ()
        {
            return this.propertyBinder;
        },
        set_PropertyBinder: function (value)
        {
            this.propertyBinder = Cast(value, System.Action$1.ctor);
        },
        BindProperties: function ()
        {
            if (this.propertyBinder != null)
            {
                this.propertyBinder(this.control);
                this.set_ArePropertiesBound(true);
            }
        }
    }
};
JsTypes.push(Neptuo$Templates$ComponentManager$ComponentEntry$1);
var Neptuo$Templates$ComponentManager$ObserverInfo =
{
    fullname: "Neptuo.Templates.ComponentManager.ObserverInfo",
    baseTypeName: "System.Object",
    assemblyName: "Neptuo.Templates.Components",
    Kind: "Class",
    definition:
    {
        ctor: function (observer, propertyBinder)
        {
            this._Observer = null;
            this._PropertyBinder = null;
            this._ArePropertiesBound = false;
            System.Object.ctor.call(this);
            this.set_Observer(observer);
            this.set_PropertyBinder(propertyBinder);
        },
        Observer$$: "Neptuo.Templates.Observers.IObserver",
        get_Observer: function ()
        {
            return this._Observer;
        },
        set_Observer: function (value)
        {
            this._Observer = value;
        },
        PropertyBinder$$: "System.Delegate",
        get_PropertyBinder: function ()
        {
            return this._PropertyBinder;
        },
        set_PropertyBinder: function (value)
        {
            this._PropertyBinder = value;
        },
        ArePropertiesBound$$: "System.Boolean",
        get_ArePropertiesBound: function ()
        {
            return this._ArePropertiesBound;
        },
        set_ArePropertiesBound: function (value)
        {
            this._ArePropertiesBound = value;
        }
    }
};
JsTypes.push(Neptuo$Templates$ComponentManager$ObserverInfo);
var Neptuo$Templates$ComponentManager$ObserverInfo$1 =
{
    fullname: "Neptuo.Templates.ComponentManager.ObserverInfo$1",
    baseTypeName: "Neptuo.Templates.ComponentManager.ObserverInfo",
    assemblyName: "Neptuo.Templates.Components",
    Kind: "Class",
    definition:
    {
        ctor: function (T, observer, propertyBinder)
        {
            this.T = T;
            this.observer = null;
            this.propertyBinder = null;
            Neptuo.Templates.ComponentManager.ObserverInfo.ctor.call(this, observer, propertyBinder);
        },
        Observer$$: "Neptuo.Templates.Observers.IObserver",
        get_Observer: function ()
        {
            return this.observer;
        },
        set_Observer: function (value)
        {
            this.observer = Cast(value, this.T);
        },
        PropertyBinder$$: "System.Delegate",
        get_PropertyBinder: function ()
        {
            return this.propertyBinder;
        },
        set_PropertyBinder: function (value)
        {
            this.propertyBinder = Cast(value, System.Action$1.ctor);
        },
        BindProperties: function ()
        {
            if (this.propertyBinder != null)
            {
                this.propertyBinder(this.observer);
                this.set_ArePropertiesBound(true);
            }
        }
    }
};
JsTypes.push(Neptuo$Templates$ComponentManager$ObserverInfo$1);
var Neptuo$Templates$Controls$ComponentAttribute =
{
    fullname: "Neptuo.Templates.Controls.ComponentAttribute",
    baseTypeName: "System.Attribute",
    assemblyName: "Neptuo.Templates.Components",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            this._Name = null;
            System.Attribute.ctor.call(this);
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
        ctor$$String: function (name)
        {
            this._Name = null;
            System.Attribute.ctor.call(this);
            this.set_Name(name);
        }
    }
};
JsTypes.push(Neptuo$Templates$Controls$ComponentAttribute);
var Neptuo$Templates$Extensions$DefaultMarkupExtensionContext =
{
    fullname: "Neptuo.Templates.Extensions.DefaultMarkupExtensionContext",
    baseTypeName: "System.Object",
    assemblyName: "Neptuo.Templates.Components",
    interfaceNames: ["Neptuo.Templates.Extensions.IValueExtensionContext"],
    Kind: "Class",
    definition:
    {
        ctor: function (targetObject, targetProperty, dependencyProvider)
        {
            this._TargetObject = null;
            this._TargetProperty = null;
            this._DependencyProvider = null;
            System.Object.ctor.call(this);
            this.set_TargetObject(targetObject);
            this.set_TargetProperty(targetProperty);
            this.set_DependencyProvider(dependencyProvider);
        },
        TargetObject$$: "System.Object",
        get_TargetObject: function ()
        {
            return this._TargetObject;
        },
        set_TargetObject: function (value)
        {
            this._TargetObject = value;
        },
        TargetProperty$$: "System.Reflection.PropertyInfo",
        get_TargetProperty: function ()
        {
            return this._TargetProperty;
        },
        set_TargetProperty: function (value)
        {
            this._TargetProperty = value;
        },
        DependencyProvider$$: "Neptuo.Templates.IDependencyProvider",
        get_DependencyProvider: function ()
        {
            return this._DependencyProvider;
        },
        set_DependencyProvider: function (value)
        {
            this._DependencyProvider = value;
        }
    }
};
JsTypes.push(Neptuo$Templates$Extensions$DefaultMarkupExtensionContext);
var Neptuo$Templates$HtmlAttribute =
{
    fullname: "Neptuo.Templates.HtmlAttribute",
    baseTypeName: "System.Attribute",
    assemblyName: "Neptuo.Templates.Components",
    Kind: "Class",
    definition:
    {
        ctor$$String: function (tagName)
        {
            this._TagName = null;
            this._IsSelfClosing = false;
            System.Attribute.ctor.call(this);
            this.set_TagName(tagName);
        },
        TagName$$: "System.String",
        get_TagName: function ()
        {
            return this._TagName;
        },
        set_TagName: function (value)
        {
            this._TagName = value;
        },
        IsSelfClosing$$: "System.Boolean",
        get_IsSelfClosing: function ()
        {
            return this._IsSelfClosing;
        },
        set_IsSelfClosing: function (value)
        {
            this._IsSelfClosing = value;
        },
        ctor$$String$$Boolean: function (tagName, isSelfClosing)
        {
            this._TagName = null;
            this._IsSelfClosing = false;
            Neptuo.Templates.HtmlAttribute.ctor$$String.call(this, tagName);
            this.set_IsSelfClosing(isSelfClosing);
        }
    }
};
JsTypes.push(Neptuo$Templates$HtmlAttribute);
var Neptuo$Templates$IAttributeCollection = {fullname: "Neptuo.Templates.IAttributeCollection", baseTypeName: "System.Object", assemblyName: "Neptuo.Templates.Components", Kind: "Interface"};
JsTypes.push(Neptuo$Templates$IAttributeCollection);
var Neptuo$Templates$Controls$IContentControl = {fullname: "Neptuo.Templates.Controls.IContentControl", baseTypeName: "System.Object", assemblyName: "Neptuo.Templates.Components", interfaceNames: ["Neptuo.Templates.Controls.IControl"], Kind: "Interface"};
JsTypes.push(Neptuo$Templates$Controls$IContentControl);
var Neptuo$Templates$Controls$IControl = {fullname: "Neptuo.Templates.Controls.IControl", baseTypeName: "System.Object", assemblyName: "Neptuo.Templates.Components", Kind: "Interface"};
JsTypes.push(Neptuo$Templates$Controls$IControl);
var Neptuo$Templates$Extensions$IValueExtension = {fullname: "Neptuo.Templates.Extensions.IValueExtension", baseTypeName: "System.Object", assemblyName: "Neptuo.Templates.Components", Kind: "Interface"};
JsTypes.push(Neptuo$Templates$Extensions$IValueExtension);
var Neptuo$Templates$Extensions$IValueExtensionContext = {fullname: "Neptuo.Templates.Extensions.IValueExtensionContext", baseTypeName: "System.Object", assemblyName: "Neptuo.Templates.Components", Kind: "Interface"};
JsTypes.push(Neptuo$Templates$Extensions$IValueExtensionContext);
var Neptuo$Templates$HtmlTextWriter =
{
    fullname: "Neptuo.Templates.HtmlTextWriter",
    baseTypeName: "System.IO.TextWriter",
    staticDefinition:
    {
        cctor: function ()
        {
        }
    },
    assemblyName: "Neptuo.Templates.Components",
    Kind: "Class",
    definition:
    {
        ctor$$TextWriter: function (writer)
        {
            this._InnerWriter = null;
            System.IO.TextWriter.ctor.call(this);
            this.set_InnerWriter(new System.Web.UI.HtmlTextWriter.ctor$$TextWriter(writer));
        },
        InnerWriter$$: "System.Web.UI.HtmlTextWriter",
        get_InnerWriter: function ()
        {
            return this._InnerWriter;
        },
        set_InnerWriter: function (value)
        {
            this._InnerWriter = value;
        },
        ctor$$TextWriter$$String: function (writer, tabString)
        {
            this._InnerWriter = null;
            System.IO.TextWriter.ctor.call(this);
            this.set_InnerWriter(new System.Web.UI.HtmlTextWriter.ctor$$TextWriter$$String(writer, tabString));
        },
        Encoding$$: "System.Text.Encoding",
        get_Encoding: function ()
        {
            return this.get_InnerWriter().get_Encoding();
        },
        Indent$$: "System.Int32",
        get_Indent: function ()
        {
            return this.get_InnerWriter().get_Indent();
        },
        set_Indent: function (value)
        {
            this.get_InnerWriter().set_Indent(value);
        },
        NewLine$$: "System.String",
        get_NewLine: function ()
        {
            return this.get_InnerWriter().get_NewLine();
        },
        set_NewLine: function (value)
        {
            this.get_InnerWriter().set_NewLine(value);
        },
        AddAttribute$$HtmlTextWriterAttribute$$String: function (key, value)
        {
            this.get_InnerWriter().AddAttribute$$HtmlTextWriterAttribute$$String(key, value);
        },
        AddAttribute$$String$$String: function (name, value)
        {
            this.get_InnerWriter().AddAttribute$$String$$String(name, value);
        },
        AddAttribute$$HtmlTextWriterAttribute$$String$$Boolean: function (key, value, fEncode)
        {
            this.get_InnerWriter().AddAttribute$$HtmlTextWriterAttribute$$String$$Boolean(key, value, fEncode);
        },
        AddAttribute$$String$$String$$Boolean: function (name, value, fEncode)
        {
            this.get_InnerWriter().AddAttribute$$String$$String$$Boolean(name, value, fEncode);
        },
        AddStyleAttribute$$HtmlTextWriterStyle$$String: function (key, value)
        {
            this.get_InnerWriter().AddStyleAttribute$$HtmlTextWriterStyle$$String(key, value);
        },
        AddStyleAttribute$$String$$String: function (name, value)
        {
            this.get_InnerWriter().AddStyleAttribute$$String$$String(name, value);
        },
        BeginRender: function ()
        {
            this.get_InnerWriter().BeginRender();
        },
        Close: function ()
        {
            this.get_InnerWriter().Close();
        },
        EndRender: function ()
        {
            this.get_InnerWriter().EndRender();
        },
        Flush: function ()
        {
            this.get_InnerWriter().Flush();
        },
        IsValidFormAttribute: function (attribute)
        {
            return this.get_InnerWriter().IsValidFormAttribute(attribute);
        },
        RenderBeginTag$$HtmlTextWriterTag: function (tagKey)
        {
            this.get_InnerWriter().RenderBeginTag$$HtmlTextWriterTag(tagKey);
        },
        RenderBeginTag$$String: function (tagName)
        {
            this.get_InnerWriter().RenderBeginTag$$String(tagName);
        },
        RenderEndTag: function ()
        {
            this.get_InnerWriter().RenderEndTag();
        },
        Write$$Boolean: function (value)
        {
            this.get_InnerWriter().Write$$Boolean(value);
        },
        Write$$Char: function (value)
        {
            this.get_InnerWriter().Write$$Char(value);
        },
        Write$$Char$Array: function (buffer)
        {
            this.get_InnerWriter().Write$$Char$Array(buffer);
        },
        Write$$Double: function (value)
        {
            this.get_InnerWriter().Write$$Double(value);
        },
        Write$$Single: function (value)
        {
            this.get_InnerWriter().Write$$Single(value);
        },
        Write$$Int32: function (value)
        {
            this.get_InnerWriter().Write$$Int32(value);
        },
        Write$$Int64: function (value)
        {
            this.get_InnerWriter().Write$$Int64(value);
        },
        Write$$Object: function (value)
        {
            this.get_InnerWriter().Write$$Object(value);
        },
        Write$$String: function (s)
        {
            this.get_InnerWriter().Write$$String(s);
        },
        Write$$String$$Object: function (format, arg0)
        {
            this.get_InnerWriter().Write$$String$$Object(format, arg0);
        },
        Write$$String$$Object$Array: function (format, arg)
        {
            this.get_InnerWriter().Write$$String$$Object$Array(format, arg);
        },
        Write$$Char$Array$$Int32$$Int32: function (buffer, index, count)
        {
            this.get_InnerWriter().Write$$Char$Array$$Int32$$Int32(buffer, index, count);
        },
        Write$$String$$Object$$Object: function (format, arg0, arg1)
        {
            this.get_InnerWriter().Write$$String$$Object$$Object(format, arg0, arg1);
        },
        WriteAttribute$$String$$String: function (name, value)
        {
            this.get_InnerWriter().WriteAttribute$$String$$String(name, value);
        },
        WriteAttribute$$String$$String$$Boolean: function (name, value, fEncode)
        {
            this.get_InnerWriter().WriteAttribute$$String$$String$$Boolean(name, value, fEncode);
        },
        WriteBeginTag: function (tagName)
        {
            this.get_InnerWriter().WriteBeginTag(tagName);
        },
        WriteBreak: function ()
        {
            this.get_InnerWriter().WriteBreak();
        },
        WriteEncodedText: function (text)
        {
            this.get_InnerWriter().WriteEncodedText(text);
        },
        WriteEncodedUrl: function (url)
        {
            this.get_InnerWriter().WriteEncodedUrl(url);
        },
        WriteEncodedUrlParameter: function (urlText)
        {
            this.get_InnerWriter().WriteEncodedUrlParameter(urlText);
        },
        WriteEndTag: function (tagName)
        {
            this.get_InnerWriter().WriteEndTag(tagName);
        },
        WriteFullBeginTag: function (tagName)
        {
            this.get_InnerWriter().WriteFullBeginTag(tagName);
        },
        WriteLine: function ()
        {
            this.get_InnerWriter().WriteLine();
        },
        WriteLine$$Boolean: function (value)
        {
            this.get_InnerWriter().WriteLine$$Boolean(value);
        },
        WriteLine$$Char: function (value)
        {
            this.get_InnerWriter().WriteLine$$Char(value);
        },
        WriteLine$$Char$Array: function (buffer)
        {
            this.get_InnerWriter().WriteLine$$Char$Array(buffer);
        },
        WriteLine$$Double: function (value)
        {
            this.get_InnerWriter().WriteLine$$Double(value);
        },
        WriteLine$$Single: function (value)
        {
            this.get_InnerWriter().WriteLine$$Single(value);
        },
        WriteLine$$Int32: function (value)
        {
            this.get_InnerWriter().WriteLine$$Int32(value);
        },
        WriteLine$$Int64: function (value)
        {
            this.get_InnerWriter().WriteLine$$Int64(value);
        },
        WriteLine$$Object: function (value)
        {
            this.get_InnerWriter().WriteLine$$Object(value);
        },
        WriteLine$$String: function (s)
        {
            this.get_InnerWriter().WriteLine$$String(s);
        },
        WriteLine$$UInt32: function (value)
        {
            this.get_InnerWriter().WriteLine$$UInt32(value);
        },
        WriteLine$$String$$Object: function (format, arg0)
        {
            this.get_InnerWriter().WriteLine$$String$$Object(format, arg0);
        },
        WriteLine$$String$$Object$Array: function (format, arg)
        {
            this.get_InnerWriter().WriteLine$$String$$Object$Array(format, arg);
        },
        WriteLine$$Char$Array$$Int32$$Int32: function (buffer, index, count)
        {
            this.get_InnerWriter().WriteLine$$Char$Array$$Int32$$Int32(buffer, index, count);
        },
        WriteLine$$String$$Object$$Object: function (format, arg0, arg1)
        {
            this.get_InnerWriter().WriteLine$$String$$Object$$Object(format, arg0, arg1);
        },
        WriteLineNoTabs: function (s)
        {
            this.get_InnerWriter().WriteLineNoTabs(s);
        },
        WriteStyleAttribute$$String$$String: function (name, value)
        {
            this.get_InnerWriter().WriteStyleAttribute$$String$$String(name, value);
        },
        WriteStyleAttribute$$String$$String$$Boolean: function (name, value, fEncode)
        {
            this.get_InnerWriter().WriteStyleAttribute$$String$$String$$Boolean(name, value, fEncode);
        }
    }
};
JsTypes.push(Neptuo$Templates$HtmlTextWriter);
var Neptuo$Templates$IComponentManager = {fullname: "Neptuo.Templates.IComponentManager", baseTypeName: "System.Object", assemblyName: "Neptuo.Templates.Components", Kind: "Interface"};
JsTypes.push(Neptuo$Templates$IComponentManager);
var Neptuo$Templates$OnInitComplete =
{
    fullname: "Neptuo.Templates.OnInitComplete",
    Kind: "Delegate",
    definition:
    {
        ctor: function (obj, func)
        {
            System.MulticastDelegate.ctor.call(this, obj, func);
        }
    }
};
JsTypes.push(Neptuo$Templates$OnInitComplete);
var Neptuo$Templates$ControlInitCompleteEventArgs =
{
    fullname: "Neptuo.Templates.ControlInitCompleteEventArgs",
    baseTypeName: "System.EventArgs",
    staticDefinition:
    {
        cctor: function ()
        {
        }
    },
    assemblyName: "Neptuo.Templates.Components",
    Kind: "Class",
    definition:
    {
        ctor: function (target)
        {
            this._Target = null;
            System.EventArgs.ctor.call(this);
            this.set_Target(target);
        },
        Target$$: "Neptuo.Templates.Controls.IControl",
        get_Target: function ()
        {
            return this._Target;
        },
        set_Target: function (value)
        {
            this._Target = value;
        }
    }
};
JsTypes.push(Neptuo$Templates$ControlInitCompleteEventArgs);
var Neptuo$Templates$IViewPage = {fullname: "Neptuo.Templates.IViewPage", baseTypeName: "System.Object", assemblyName: "Neptuo.Templates.Components", interfaceNames: ["Neptuo.Templates.Controls.IContentControl", "System.IDisposable"], Kind: "Interface"};
JsTypes.push(Neptuo$Templates$IViewPage);
var Neptuo$Templates$IVirtualUrlProvider = {fullname: "Neptuo.Templates.IVirtualUrlProvider", baseTypeName: "System.Object", assemblyName: "Neptuo.Templates.Components", Kind: "Interface"};
JsTypes.push(Neptuo$Templates$IVirtualUrlProvider);
var Neptuo$Templates$IVirtualPathProvider = {fullname: "Neptuo.Templates.IVirtualPathProvider", baseTypeName: "System.Object", assemblyName: "Neptuo.Templates.Components", Kind: "Interface"};
JsTypes.push(Neptuo$Templates$IVirtualPathProvider);
var Neptuo$Templates$ObserverAttribute =
{
    fullname: "Neptuo.Templates.ObserverAttribute",
    baseTypeName: "System.Attribute",
    assemblyName: "Neptuo.Templates.Components",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            this._Livecycle = Neptuo.Templates.ObserverLivecycle.PerAttribute;
            Neptuo.Templates.ObserverAttribute.ctor$$ObserverLivecycle.call(this, Neptuo.Templates.ObserverLivecycle.PerControl);
        },
        Livecycle$$: "Neptuo.Templates.ObserverLivecycle",
        get_Livecycle: function ()
        {
            return this._Livecycle;
        },
        set_Livecycle: function (value)
        {
            this._Livecycle = value;
        },
        ctor$$ObserverLivecycle: function (livecycle)
        {
            this._Livecycle = Neptuo.Templates.ObserverLivecycle.PerAttribute;
            System.Attribute.ctor.call(this);
            this.set_Livecycle(livecycle);
        }
    }
};
JsTypes.push(Neptuo$Templates$ObserverAttribute);
var Neptuo$Templates$ObserverLivecycle =
{
    fullname: "Neptuo.Templates.ObserverLivecycle",
    staticDefinition: {PerAttribute: "PerAttribute", PerControl: "PerControl", PerPage: "PerPage"},
    Kind: "Enum"
};
JsTypes.push(Neptuo$Templates$ObserverLivecycle);
var Neptuo$Templates$Observers$IObserver = {fullname: "Neptuo.Templates.Observers.IObserver", baseTypeName: "System.Object", assemblyName: "Neptuo.Templates.Components", Kind: "Interface"};
JsTypes.push(Neptuo$Templates$Observers$IObserver);
var Neptuo$Templates$Observers$ObserverEventArgs =
{
    fullname: "Neptuo.Templates.Observers.ObserverEventArgs",
    baseTypeName: "System.EventArgs",
    staticDefinition:
    {
        cctor: function ()
        {
        }
    },
    assemblyName: "Neptuo.Templates.Components",
    Kind: "Class",
    definition:
    {
        ctor: function (target)
        {
            this._Target = null;
            this._Cancel = false;
            System.EventArgs.ctor.call(this);
            this.set_Target(target);
        },
        Target$$: "Neptuo.Templates.Controls.IControl",
        get_Target: function ()
        {
            return this._Target;
        },
        set_Target: function (value)
        {
            this._Target = value;
        },
        Cancel$$: "System.Boolean",
        get_Cancel: function ()
        {
            return this._Cancel;
        },
        set_Cancel: function (value)
        {
            this._Cancel = value;
        }
    }
};
JsTypes.push(Neptuo$Templates$Observers$ObserverEventArgs);
var Neptuo$Templates$PropertyAttribute =
{
    fullname: "Neptuo.Templates.PropertyAttribute",
    baseTypeName: "System.Attribute",
    assemblyName: "Neptuo.Templates.Components",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            this._Name = null;
            System.Attribute.ctor.call(this);
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
        ctor$$String: function (name)
        {
            this._Name = null;
            System.Attribute.ctor.call(this);
            this.set_Name(name);
        }
    }
};
JsTypes.push(Neptuo$Templates$PropertyAttribute);
