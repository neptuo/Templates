﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Control.
    /// </summary>
    public class ComponentCodeObject : ITypeCodeObject, IPropertiesCodeObject, IObserversCodeObject
    {
        public Type Type { get; set; }
        public List<ICodeProperty> Properties { get; set; }
        public List<ICodeObject> Observers { get; set; }

        public ComponentCodeObject(Type type)
        {
            Type = type;
            Properties = new List<ICodeProperty>();
            Observers = new List<ICodeObject>();
        }
    }
}
