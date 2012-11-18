﻿using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.PreProcessing
{
    public interface IPreProcessor
    {
        void Process(IPropertyDescriptor propertyDescriptor, IPreProcessorContext context);
    }
}
