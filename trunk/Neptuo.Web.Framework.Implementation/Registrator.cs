using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Utils;
using Neptuo.Web.Framework.Observers;

namespace Neptuo.Web.Framework
{
    public class Registrator : IRegistrator
    {
        public const string ObserverWildcard = "*";

        public Dictionary<string, List<string>> Namespaces { get; protected set; }
        public Dictionary<string, Dictionary<string, Type>> ControlsInNamespaces { get; protected set; }
        public Dictionary<string, Dictionary<string, Type>> ExtensionsInNamespaces { get; protected set; }
        public Dictionary<string, Dictionary<string, Type>> Observers { get; protected set; }

        public Registrator(Dictionary<string, List<string>> namespaces, Dictionary<string, Dictionary<string, Type>> controlsInNamespaces, 
            Dictionary<string, Dictionary<string, Type>> extensionsInNamespaces, Dictionary<string, Dictionary<string, Type>> observers)
        {
            Namespaces = namespaces;
            ControlsInNamespaces = controlsInNamespaces;
            ExtensionsInNamespaces = extensionsInNamespaces;
            Observers = observers;
        }

        public Registrator()
            : this(new Dictionary<string, List<string>>(), new Dictionary<string, Dictionary<string, Type>>(), new Dictionary<string, Dictionary<string, Type>>(), new Dictionary<string, Dictionary<string, Type>>())
        { }

        public Type GetControl(string tagNamespace, string tagName)
        {
            if (tagNamespace == null)
                tagNamespace = String.Empty;
            else
                tagNamespace = tagNamespace.ToLowerInvariant();

            tagName = tagName.ToLowerInvariant();

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
            if (tagNamespace == null)
                tagNamespace = String.Empty;
            else
                tagNamespace = tagNamespace.ToLowerInvariant();

            tagName = tagName.ToLowerInvariant();

            if (ExtensionsInNamespaces.ContainsKey(tagNamespace)
                && ExtensionsInNamespaces[tagNamespace].ContainsKey(tagName))
            {
                Type controlType = ExtensionsInNamespaces[tagNamespace][tagName];
                return controlType;
            }

            return null;
        }

        public Type GetObserver(string attributeNamespace, string attributeName)
        {
            if (attributeNamespace == null)
                attributeNamespace = String.Empty;
            else
                attributeNamespace = attributeNamespace.ToLowerInvariant();

            attributeName = attributeName.ToLowerInvariant();

            if (!Observers.ContainsKey(attributeNamespace))
                return null;

            if (Observers[attributeNamespace].ContainsKey(attributeName))
                return Observers[attributeNamespace][attributeName];

            if (Observers[attributeNamespace].ContainsKey(ObserverWildcard))
                return Observers[attributeNamespace][ObserverWildcard];

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
                if (ReflectionHelper.CanBeUsedInMarkup(type, false))
                {
                    ComponentAttribute controlAttr = ReflectionHelper.GetAttribute<ComponentAttribute>(type);

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
                if (ReflectionHelper.CanBeUsedInMarkup(type, false))
                {
                    ComponentAttribute controlAttr = ReflectionHelper.GetAttribute<ComponentAttribute>(type);

                    string extensionName = type.Name.ToLowerInvariant();
                    if (extensionName.EndsWith("extension"))
                        extensionName = extensionName.Substring(0, extensionName.Length - 9);

                    if (controlAttr != null && !String.IsNullOrEmpty(controlAttr.Name))
                        extensionName = controlAttr.Name;

                    ExtensionsInNamespaces[prefix][extensionName] = type;
                }
            }
        }

        public void RegisterObserver(string prefix, string attributePattern, Type observer)
        {
            if (!typeof(IObserver).IsAssignableFrom(observer))
                throw new ApplicationException("This type is not an observer!");

            if (String.IsNullOrEmpty(attributePattern))
                attributePattern = ObserverWildcard;

            prefix = prefix.ToLowerInvariant();
            attributePattern = attributePattern.ToLowerInvariant();

            if (!Observers.ContainsKey(prefix))
                Observers.Add(prefix, new Dictionary<string, Type>());

            if (Observers[prefix].ContainsKey(attributePattern))
                Observers[prefix].Add(attributePattern, observer);
            else
                Observers[prefix][attributePattern] = observer;
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

            foreach (KeyValuePair<string, Dictionary<string, Type>> item in Observers)
            {
                yield return new RegisteredNamespace
                {
                    Prefix = item.Key,
                    Namespace = null
                };
            }
        }

        public IRegistrator CreateChildRegistrator()
        {
            return new Registrator(
                new Dictionary<string, List<string>>(Namespaces),
                new Dictionary<string, Dictionary<string, Type>>(ControlsInNamespaces),
                new Dictionary<string, Dictionary<string, Type>>(ExtensionsInNamespaces),
                new Dictionary<string, Dictionary<string, Type>>(Observers)
            );
        }
    }
}
