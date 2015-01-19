using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Generator for processing attributes used on classes or properties.
    /// </summary>
    public interface ICodeDomAttributeGenerator
    {
        /// <summary>
        /// Generates expression for 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        ICodeDomAttributeResult Generate(ICodeDomContext context, Attribute attribute);
    }
}
