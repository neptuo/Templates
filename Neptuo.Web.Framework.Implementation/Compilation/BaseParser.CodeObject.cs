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
        public abstract class CodeObject : ICodeObject
        {
            public Dictionary<string, ICodeObject> Properties { get; set; }
            public IPropertyInfo PropertyInfo { get; set; }

            public CodeObject()
            {
                Properties = new Dictionary<string, ICodeObject>();
            }

            /// Přidá property do objektu -> předem musí být nastavenou jakou formou!
            public void AddProperty(ICodeObject codeObject)
            {
                PropertyInfo.SetProperty(this, codeObject);
            }

            public abstract void Generate(ICodeGenerator codeGenerator, ICodeObjectContext context);
        }

        /// <summary>
        /// Hodnota property, v již hotovém tvaru.
        /// </summary>
        public class PlainValueCodeObject : ICodeObject
        {
            public object Value { get; set; }

            public PlainValueCodeObject(object value)
            {
                Value = value;
            }

            public void AddProperty(ICodeObject propertyObject)
            {
                throw new NotImplementedException();//Nenastane!
            }

            public void Generate(ICodeGenerator codeGenerator, ICodeObjectContext context)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Control.
        /// </summary>
        public class ControlCodeObject : CodeObject, IControlCodeObject
        {
            public Type Type { get; set; }
            public List<IObserverCodeObject> Observers { get; set; }

            public ControlCodeObject(Type type)
            {
                Type = type;
                Observers = new List<IObserverCodeObject>();
            }

            public override void Generate(ICodeGenerator codeGenerator, ICodeObjectContext context)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Observer.
        /// </summary>
        public class ObserverCodeObject : CodeObject, IObserverCodeObject
        {
            public Type Type { get; set; }
            public ObserverInfo Info { get; set; }

            public override void Generate(ICodeGenerator codeGenerator, ICodeObjectContext context)
            {
                throw new NotImplementedException();
            }

            public class ObserverInfo
            {
                public ObserverLivecycle Livecycle { get; protected set; }

                public ObserverInfo(ObserverLivecycle livecycle)
                {
                    Livecycle = livecycle;
                }
            }
        }


        /// <summary>
        /// Kořenový obalový element.
        /// </summary>
        public class RootCodeObject : CodeObject, IRootCodeObject
        {
            public new List<ICodeObject> Properties { get; set; }

            public RootCodeObject()
            {
                Properties = new List<ICodeObject>();
                PropertyInfo = new RootPropertyInfo();
            }

            public override void Generate(ICodeGenerator codeGenerator, ICodeObjectContext context)
            {
                foreach (ICodeObject codeObject in Properties)
                    codeObject.Generate(codeGenerator, new CodeObjectContext(this));
            }

            private class RootPropertyInfo : IPropertyInfo
            {
                public Type RequiredType
                {
                    get { return typeof(object); }
                }

                public void SetProperty(CodeObject parentObject, ICodeObject codeObject)
                {
                    ((RootCodeObject)parentObject).Properties.Add(codeObject);
                }
            }
        }

        /// <summary>
        /// Nastavení property typu list.
        /// TODO: Přidat třídu pro podporu IDictiornary
        /// </summary>
        public class ListValueCodeObject : CodeObject
        {
            public List<ICodeObject> Values { get; set; }

            public ListValueCodeObject()
            {
                Values = new List<ICodeObject>();
            }

            public override void Generate(ICodeGenerator codeGenerator, ICodeObjectContext context)
            {
                //Nepředávat sebe jak rodiče, ale předávat "pra rodiče" (aby bylo možné zjistit o jakou property jde).
                throw new NotImplementedException();
            }
        }
    }
}
