using Neptuo.Models.Features;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public abstract class CodeDomObjectGeneratorBase<T> : ICodeDomObjectGenerator
        where T : ICodeObject
    {
        public virtual ICodeDomObjectResult Generate(ICodeDomObjectContext context, ICodeObject codeObject)
        {
            if (codeObject is T)
                return Generate(context, (T)codeObject);

            IFeatureModel featureObject = codeObject as IFeatureModel;
            if (featureObject != null)
                return Generate(context, featureObject.With<T>());

            throw new NotSupportedException();
        }

        protected abstract ICodeDomObjectResult Generate(ICodeDomObjectContext context, T codeObject);
    }
}
