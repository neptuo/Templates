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
        public Dictionary<string, Dictionary<string, Type>> ControlsInNamespaces { get; protected set; }
        public Dictionary<string, Dictionary<string, Type>> ExtensionsInNamespaces { get; protected set; }

        public Registrator()
        {
            Namespaces = new Dictionary<string, List<string>>();
            ControlsInNamespaces = new Dictionary<string, Dictionary<string, Type>>();
            ExtensionsInNamespaces = new Dictionary<string, Dictionary<string, Type>>();
        }

        public Type GetControl(string tagNamespace, string tagName)
        {
            if (ControlsInNamespaces.ContainsKey(tagNamespace)
                && ControlsInNamespaces[tagNamespace].ContainsKey(tagName))
            {
                Type controlType = ControlsInNamespaces[tagNamespace][tagName];
                return controlType;
            }

            return null;
        }

        public Type GetExtension(string tagNamespace, string tagName)
        {
            if (ExtensionsInNamespaces.ContainsKey(tagNamespace)
                && ExtensionsInNamespaces[tagNamespace].ContainsKey(tagName))
            {
                Type controlType = ExtensionsInNamespaces[tagNamespace][tagName];
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

            if (!ControlsInNamespaces.ContainsKey(prefix))
                ControlsInNamespaces.Add(prefix, new Dictionary<string, Type>());

            if (!ExtensionsInNamespaces.ContainsKey(prefix))
                ExtensionsInNamespaces.Add(prefix, new Dictionary<string, Type>());

            List<Type> types = ReflectionHelper.GetTypesInNamespace(newNamespace);

            RegisterControls(types, prefix);
            RegisterExtensions(types, prefix);
        }

        private void RegisterControls(List<Type> types, string prefix)
        {
            foreach (Type type in types)
            {
                if (ReflectionHelper.CanBeUsedInMarkup(type))
                {
                    ControlAttribute controlAttr = ControlAttribute.GetAttribute(type);

                    string controlName = type.Name.ToLowerInvariant();
                    if (controlName.EndsWith("control"))
                        controlName = controlName.Substring(0, controlName.Length - 7);

                    if (controlAttr != null && !String.IsNullOrEmpty(controlAttr.Name))
                        controlName = controlAttr.Name;

                    ControlsInNamespaces[prefix][controlName] = type;
                }
            }
        }

        private void RegisterExtensions(List<Type> types, string prefix)
        {
            foreach (Type type in types)
            {
                if (ReflectionHelper.CanBeUsedInMarkup(type))
                {
                    ControlAttribute controlAttr = ControlAttribute.GetAttribute(type);

                    string extensionName = type.Name.ToLowerInvariant();
                    if (extensionName.EndsWith("extension"))
                        extensionName = extensionName.Substring(0, extensionName.Length - 9);

                    if (controlAttr != null && !String.IsNullOrEmpty(controlAttr.Name))
                        extensionName = controlAttr.Name;

                    ExtensionsInNamespaces[prefix][extensionName] = type;
                }
            }
        }

        public IEnumerable<RegisteredNamespace> GetRegisteredNamespaces()
        {
            foreach (KeyValuePair<string, List<string>> entry in Namespaces)
            {
                foreach (string item in entry.Value)
                {
                    yield return new RegisteredNamespace
                    {
                        Prefix = entry.Key,
                        Namespace = item
                    };
                }
            }
        }
    }
}
