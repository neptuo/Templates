using Neptuo.ComponentModel;
using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Implementation of object generator for <see cref="IObserverCodeObject"/> using component wrap
    /// and delegation to <see cref="CodeDomComponentObjectGenerator"/>.
    /// Takes instance of <see cref="IObserverCodeObject"/>, wraps it to <see cref="ComponentCodeObject"/>
    /// and generates code using <see cref="CodeDomComponentObjectGenerator"/>.
    /// </summary>
    public class CodeDomObserverObjectGenerator : ICodeDomObjectGenerator
    {
        private readonly CodeDomComponentObjectGenerator generator;

        public CodeDomObserverObjectGenerator(IUniqueNameProvider nameProvider)
        {
            generator = new CodeDomComponentObjectGenerator(nameProvider);
        }

        public ICodeDomObjectResult Generate(ICodeDomObjectContext context, ICodeObject codeObject)
        {
            IObserverCodeObject observer = (IObserverCodeObject)codeObject;
            ComponentCodeObject component = new ComponentCodeObject(observer.Type);
            component.Properties.AddRange(observer.Properties);

            ICodeDomObjectResult result = generator.Generate(context, component);
            if (result == null)
                return result;

            string variableName;
            if (!context.TryGetObserverTarget(out variableName))
            {
                context.AddError("Unnable to process observer without target variable name.");
                return null;
            }

            if (!result.HasExpression())
            {
                context.AddError("Unnable to process observer without expression from generated component.");
                return null;
            }

            return new CodeDomDefaultObjectResult(

            );
        }
    }
}
