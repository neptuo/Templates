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
if (typeof(JsTypes) == "undefined")
    var JsTypes = [];
var Neptuo$Templates$DependencyAttribute =
{
    fullname: "Neptuo.Templates.DependencyAttribute",
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
JsTypes.push(Neptuo$Templates$DependencyAttribute);
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
        },
        CreateValueExtensionContext: function (targetObject, targetProperty)
        {
            return new Neptuo.Templates.Extensions.DefaultMarkupExtensionContext.ctor(targetObject, targetObject.GetType().GetProperty$$String(targetProperty), this.dependencyProvider);
        },
        CastValueTo$1: function (T, value)
        {
            if (value == null)
                return Default(T);
            var sourceType = value.GetType();
            var targetType = Typeof(T);
            if (sourceType == targetType)
                return Cast(value, T);
            var converter = System.ComponentModel.TypeDescriptor.GetConverter$$Object(value);
            if (converter.CanConvertTo$$Type(targetType))
                return Cast(converter.ConvertTo$$Object$$Type(value, targetType), T);
            throw $CreateException(new System.InvalidOperationException.ctor$$String(System.String.Format$$String$$Object$$Object("Unnable to convert to target type! Source type: {0}, target type: {1}", sourceType, targetType)), new Error());
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
            if (entry.get_InitComplete().get_Count() > 0)
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
            if (control.GetType().get_FullName() == Typeof(System.String.ctor).get_FullName())
                writer.Content$$Object(control);
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
        DependencyProvider$$: "Neptuo.IDependencyProvider",
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
var Neptuo$Templates$HtmlTextWriter =
{
    fullname: "Neptuo.Templates.HtmlTextWriter",
    baseTypeName: "System.Object",
    assemblyName: "Neptuo.Templates.Components",
    interfaceNames: ["Neptuo.Templates.IHtmlWriter"],
    Kind: "Class",
    definition:
    {
        ctor: function (innerWriter)
        {
            this._InnerWriter = null;
            this._OpenTags = null;
            this._IsOpenTag = false;
            this._HasContent = false;
            this._CanWriteAttribute = false;
            System.Object.ctor.call(this);
            this.set_InnerWriter(innerWriter);
            this.set_OpenTags(new System.Collections.Generic.Stack$1.ctor(System.String.ctor));
        },
        InnerWriter$$: "System.IO.TextWriter",
        get_InnerWriter: function ()
        {
            return this._InnerWriter;
        },
        set_InnerWriter: function (value)
        {
            this._InnerWriter = value;
        },
        OpenTags$$: "System.Collections.Generic.Stack`1[[System.String]]",
        get_OpenTags: function ()
        {
            return this._OpenTags;
        },
        set_OpenTags: function (value)
        {
            this._OpenTags = value;
        },
        IsOpenTag$$: "System.Boolean",
        get_IsOpenTag: function ()
        {
            return this._IsOpenTag;
        },
        set_IsOpenTag: function (value)
        {
            this._IsOpenTag = value;
        },
        HasContent$$: "System.Boolean",
        get_HasContent: function ()
        {
            return this._HasContent;
        },
        set_HasContent: function (value)
        {
            this._HasContent = value;
        },
        CanWriteAttribute$$: "System.Boolean",
        get_CanWriteAttribute: function ()
        {
            return this._CanWriteAttribute;
        },
        set_CanWriteAttribute: function (value)
        {
            this._CanWriteAttribute = value;
        },
        Content$$Object: function (content)
        {
            this.EnsureCloseOpeningTag();
            this.get_InnerWriter().Write$$Object(content);
            return this;
        },
        Content$$String: function (content)
        {
            this.EnsureCloseOpeningTag();
            this.get_InnerWriter().Write$$String(content);
            return this;
        },
        Tag: function (name)
        {
            this.EnsureCloseOpeningTag();
            this.set_CanWriteAttribute(true);
            this.set_HasContent(false);
            this.set_IsOpenTag(true);
            this.get_OpenTags().Push(name);
            this.get_InnerWriter().Write$$Char("<");
            this.get_InnerWriter().Write$$String(name);
            return this;
        },
        CloseTag: function ()
        {
            this.WriteCloseTag(this.get_HasContent());
            return this;
        },
        CloseFullTag: function ()
        {
            this.WriteCloseTag(true);
            return this;
        },
        Attribute: function (name, value)
        {
            if (!this.get_CanWriteAttribute())
                throw $CreateException(new Neptuo.Templates.HtmlTextWriterException.ctor("Unnable to write attribute in current state!"), new Error());
            this.get_InnerWriter().Write$$Char(" ");
            this.get_InnerWriter().Write$$String(name);
            this.get_InnerWriter().Write$$Char("=");
            this.get_InnerWriter().Write$$Char("\"");
            this.get_InnerWriter().Write$$String(value);
            this.get_InnerWriter().Write$$Char("\"");
            return this;
        },
        WriteCloseTag: function (hasContent)
        {
            if (this.get_OpenTags().get_Count() == 0)
                throw $CreateException(new Neptuo.Templates.HtmlTextWriterException.ctor("Unnable to close tag! All tags has been closed."), new Error());
            var name = this.get_OpenTags().Pop();
            if (hasContent)
            {
                this.EnsureCloseOpeningTag();
                this.get_InnerWriter().Write$$Char("<");
                this.get_InnerWriter().Write$$Char("/");
                this.get_InnerWriter().Write$$String(name);
                this.get_InnerWriter().Write$$Char(">");
            }
            else
            {
                this.set_IsOpenTag(false);
                this.get_InnerWriter().Write$$Char(" ");
                this.get_InnerWriter().Write$$Char("/");
                this.get_InnerWriter().Write$$Char(">");
            }
            this.set_HasContent(true);
        },
        EnsureCloseOpeningTag: function ()
        {
            if (this.get_IsOpenTag())
                this.get_InnerWriter().Write$$Char(">");
            this.set_IsOpenTag(false);
            this.set_CanWriteAttribute(false);
            this.set_HasContent(true);
        }
    }
};
JsTypes.push(Neptuo$Templates$HtmlTextWriter);
var Neptuo$Templates$HtmlTextWriter$Html =
{
    fullname: "Neptuo.Templates.HtmlTextWriter.Html",
    baseTypeName: "System.Object",
    staticDefinition:
    {
        cctor: function ()
        {
            Neptuo.Templates.HtmlTextWriter.Html.StartTag = "<";
            Neptuo.Templates.HtmlTextWriter.Html.CloseTag = ">";
            Neptuo.Templates.HtmlTextWriter.Html.Slash = "/";
            Neptuo.Templates.HtmlTextWriter.Html.Space = " ";
            Neptuo.Templates.HtmlTextWriter.Html.Equal = "=";
            Neptuo.Templates.HtmlTextWriter.Html.DoubleQuote = "\"";
            Neptuo.Templates.HtmlTextWriter.Html.Quote = "\'";
        }
    },
    assemblyName: "Neptuo.Templates.Components",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            System.Object.ctor.call(this);
        }
    }
};
JsTypes.push(Neptuo$Templates$HtmlTextWriter$Html);
var Neptuo$Templates$HtmlTextWriterException =
{
    fullname: "Neptuo.Templates.HtmlTextWriterException",
    baseTypeName: "System.Exception",
    assemblyName: "Neptuo.Templates.Components",
    Kind: "Class",
    definition:
    {
        ctor: function (message)
        {
            System.Exception.ctor$$String.call(this, message);
        }
    }
};
JsTypes.push(Neptuo$Templates$HtmlTextWriterException);
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
var Neptuo$Templates$IHtmlWriter = {fullname: "Neptuo.Templates.IHtmlWriter", baseTypeName: "System.Object", assemblyName: "Neptuo.Templates.Components", Kind: "Interface"};
JsTypes.push(Neptuo$Templates$IHtmlWriter);
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
