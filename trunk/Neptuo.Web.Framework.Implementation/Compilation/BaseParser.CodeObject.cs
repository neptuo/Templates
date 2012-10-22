using Neptuo.Web.Framework.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Web.Framework.Compilation
{
    partial class BaseParser
    {
        /// <summary>
        /// Property value (in final state) or literal object.
        /// </summary>
        public class PlainValueCodeObject : IPlainValueCodeObject
        {
            public object Value { get; set; }

            public PlainValueCodeObject(object value)
            {
                Value = value;
            }
        }

        /// <summary>
        /// Control.
        /// </summary>
        public class ControlCodeObject : IControlCodeObject
        {
            public Type Type { get; set; }
            public List<IPropertyDescriptor> Properties { get; set; }
            public List<IObserverCodeObject> Observers { get; set; }

            public ControlCodeObject(Type type)
            {
                Type = type;
                Properties = new List<IPropertyDescriptor>();
                Observers = new List<IObserverCodeObject>();
            }
        }

        /// <summary>
        /// Observer.
        /// </summary>
        public class ObserverCodeObject : IObserverCodeObject
        {
            public Type Type { get; set; }
            public List<IPropertyDescriptor> Properties { get; set; }

            public ObserverCodeObject(Type type)
            {
                Type = type;
                Properties = new List<IPropertyDescriptor>();
            }
        }

        /// <summary>
        /// Nastavení property typu list.
        /// TODO: Přidat třídu pro podporu IDictiornary
        /// </summary>
        public class ListValueCodeObject : IPropertyDescriptor
        {
            public List<ICodeObject> Values { get; set; }

            public ListValueCodeObject()
            {
                Values = new List<ICodeObject>();
            }

            public override void Generate(CodeDomGenerator codeGenerator, ICodeObjectContext context)
            {
                //Nepředávat sebe jak rodiče, ale předávat "pra rodiče" (aby bylo možné zjistit o jakou property jde).
                throw new NotImplementedException();
            }
        }
    }
}
