using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    //public interface ICodeObject
    //{
    //    //ITypeDescriptor Type { get; }
    //    //Dictionary<IPropertyDescriptor, ICodeObject> Properties { get; }
    //    //IEnumerable<ICodeObserver> Observers { get; }

    //    void AddProperty(ICodeObject propertyObject);

    //    void Generate(ICodeGenerator codeGenerator, ICodeObjectContext context);
    //}

    //public interface ICodeObjectContext
    //{
    //    ICodeObject ParentObject { get; }
    //}



    public interface ICodeObject
    {

    }

    public interface IPropertiesCodeObject : ICodeObject
    {
        List<IPropertyDescriptor> Properties { get; set; }
    }

    public interface IPlainValueCodeObject : ICodeObject
    {
        object Value { get; set; }
    }

    public interface IControlCodeObject : IPropertiesCodeObject
    {
        Type Type { get; set; }
        List<IObserverCodeObject> Observers { get; set; }
    }

    public interface IObserverCodeObject : IPropertiesCodeObject
    {
        Type Type { get; set; }
    }

    public interface IPropertyDescriptor
    {
        PropertyInfo Property { get; set; }

        void SetValue(ICodeObject value);
    }



    //public interface ICodeObserver
    //{
    //    //ITypeDescriptor Type { get; }
    //    //IObserverDescriptor Descriptor { get; }
    //    //Dictionary<IPropertyDescriptor, ICodeObject> Properties { get; }

    //    void Generate(ICodeGenerator codeGenerator);
    //}

    //public interface ITypeDescriptor
    //{
    //    string Name { get; }
    //}

    //public interface IObserverDescriptor
    //{
    //    ObserverLivecycle Livecycle { get; }
    //}
}
