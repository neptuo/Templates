using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class DefaultControlBuilder : BaseControlBuilder
    {
        protected IComponentRegistry ComponentRegistry { get; set; }

        public DefaultControlBuilder(IComponentRegistry componentRegistry)
        {
            ComponentRegistry = componentRegistry;
        }

        protected override Type GetControlType(XmlElement element)
        {
            return ComponentRegistry.GetComponentType(element.Prefix, element.LocalName);
        }
    }
}
