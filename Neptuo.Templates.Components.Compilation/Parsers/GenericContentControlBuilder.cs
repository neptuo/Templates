﻿using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class GenericContentControlBuilder : BaseControlBuider
    {
        private Type controlType;
        private string tagNameProperty;

        public GenericContentControlBuilder(Type controlType, string tagNameProperty)
        {
            this.controlType = controlType;
            this.tagNameProperty = tagNameProperty;
        }

        protected override Type GetControlType(XmlElement element)
        {
            return controlType;
        }

        protected override IComponentCodeObject CreateCodeObject(IBuilderContext context, XmlElement element)
        {
            ControlCodeObject codeObject = new ControlCodeObject(GetControlType(element));
            codeObject.Properties.Add(
                new SetPropertyDescriptor(
                    controlType.GetProperty(tagNameProperty),
                    new PlainValueCodeObject(element.Name)
                )
            );
            return codeObject;
        }
    }
}
