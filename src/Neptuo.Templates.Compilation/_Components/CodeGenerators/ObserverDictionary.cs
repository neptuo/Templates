using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class ObserverDictionary : Dictionary<Type, Dictionary<IComponentCodeObject, TypeCodeDomObjectGenerator<ComponentCodeObject>.ComponentMethodInfo>>
    { }
}
