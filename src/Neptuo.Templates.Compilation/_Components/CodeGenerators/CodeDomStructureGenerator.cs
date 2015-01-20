using Neptuo.Linq.Expressions;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomStructureGenerator
    {
        public static partial class Names
        {
            public const string CodeNamespace = "Neptuo.Templates";
            public const string ComponentManagerField = "componentManager";
            public const string DependencyProviderField = "dependencyProvider";
            public const string EntryPointFieldName = "view";

            public const string CreateViewPageControlsMethod = "BindView";
            public const string CreateValueExtensionContextMethod = "CreateValueExtensionContext";
            public const string CastValueToMethod = "CastValueTo";
        }
    }
}
