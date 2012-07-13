using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Annotations;
using Neptuo.Web.Framework.Utils;

namespace Neptuo.Web.Framework
{
    public class Registrator : IRegistrator
    {
        public Dictionary<string, List<string>> Namespaces { get; protected set; }
        public Dictionary<string, Dictionary<string, Type>> TypesInNamespaces { get; protected set; }

        public Registrator()
        {
            Namespaces = new Dictionary<string, List<string>>();
            TypesInNamespaces = new Dictionary<string, Dictionary<string, Type>>();
        }

        public Type GetControl(string tagNamespace, string tagName)
        {
            if (TypesInNamespaces.ContainsKey(tagNamespace)
                && TypesInNamespaces[tagNamespace].ContainsKey(tagName))
            {
                Type controlType = TypesInNamespaces[tagNamespace][tagName];
                return controlType;
            }

            return null;
        }

        public void RegisterNamespace(string prefix, string newNamespace)
        {
            if (prefix == null)
                prefix = String.Empty;

            if (!Namespaces.ContainsKey(prefix))
                Namespaces.Add(prefix, new List<string>());

            Namespaces[prefix].Add(newNamespace);

            if (!TypesInNamespaces.ContainsKey(prefix))
                TypesInNamespaces.Add(prefix, new Dictionary<string, Type>());

            foreach (Type type in ReflectionHelper.GetTypesInNamespace(newNamespace))
            {
                if (ReflectionHelper.CanBeUsedInMarkup(type))
                {
                    ControlAttribute controlAttr = ControlAttribute.GetAttribute(type);

                    string controlName = type.Name.ToLowerInvariant();
                    if (controlName.EndsWith("control"))
                        controlName = controlName.Substring(0, controlName.Length - 7);
                    else if (controlName.EndsWith("extension"))
                        controlName = controlName.Substring(0, controlName.Length - 9);

                    if (controlAttr != null)
                        controlName = controlAttr.Name;

                    TypesInNamespaces[prefix][controlName] = type;
                }
            }
        }
    }
}
