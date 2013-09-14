﻿using Neptuo.Templates.Compilation.CodeGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation
{
    public abstract class BaseNamingService : INamingService
    {
        public const string ViewNameFormat = "View_{0}";

        public INaming FromFile(string fileName)
        {
            return GetNaming(GetNameForFile(fileName));
        }

        public INaming FromContent(string viewContent)
        {
            return GetNaming(GetNameForContent(viewContent));
        }

        protected virtual INaming GetNaming(string viewName)
        {
            return new DefaultNaming(
                CodeDomStructureGenerator.Names.CodeNamespace,
                viewName,
                GetAssemblyForName(viewName)
            );
        }

        protected abstract string GetNameForFile(string fileName);

        protected abstract string GetNameForContent(string viewContent);

        protected virtual string GetAssemblyForName(string viewName)
        {
            return String.Format("{0}.dll", viewName);
        }
    }
}