using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class ComponentManagerDescriptor
    {
        /// <summary>
        /// Name of the method which takes component (of type T) and method (delegate with parametr of type T) to register component and it's bind method.
        /// </summary>
        public string AddComponentMethodName { get; private set; }

        /// <summary>
        /// Name of the method which takes one argument, the object to initialize.
        /// </summary>
        public string InitMethodName { get; private set; }

        /// <summary>
        /// Name of the method which takes component manager and observer to register observer of component.
        /// </summary>
        public string AttachObserverMethodName { get; private set; }

        /// <summary>
        /// Name of the method which on value extension provides value of the extension.
        /// </summary>
        public string ProvideValeExtensionMethodName { get; private set; }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="addComponentMethodName">
        /// Name of the method which takes component (of type T) and method (delegate with parametr of type T) to register component and it's bind method.
        /// </param>
        /// <param name="attachObserverMethodName">
        /// Name of the method which takes component manager and observer to register observer of component.
        /// </param>
        /// <param name="provideValeExtensionMethodName">
        /// Name of the method which on value extension provides value of the extension.
        /// </param>
        public ComponentManagerDescriptor(string addComponentMethodName, string initMethodName, string attachObserverMethodName, string provideValeExtensionMethodName)
        {
            Guard.NotNullOrEmpty(addComponentMethodName, "addComponentMethodName");
            Guard.NotNullOrEmpty(initMethodName, "initMethodName");
            Guard.NotNullOrEmpty(attachObserverMethodName, "attachObserverMethodName");
            Guard.NotNullOrEmpty(provideValeExtensionMethodName, "provideValeExtensionMethodName");
            AddComponentMethodName = addComponentMethodName;
            InitMethodName = initMethodName;
            AttachObserverMethodName = attachObserverMethodName;
            ProvideValeExtensionMethodName = provideValeExtensionMethodName;
        }
    }
}
