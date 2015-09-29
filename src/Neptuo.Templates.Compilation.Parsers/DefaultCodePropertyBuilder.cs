using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.SyntaxTrees;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Default implementation of <see cref="ICodePropertyBuilder"/>.
    /// If property is enumerable, <see cref="AddCodeProperty"/> is created; otherwise <see cref="SetCodeProperty"/> is created.
    /// </summary>
    public class DefaultCodePropertyBuilder : ICodePropertyBuilder
    {
        protected bool IsCollectionProperty(Type propertyType)
        {
            if (typeof(string) == propertyType)
                return false;

            return typeof(IEnumerable).IsAssignableFrom(propertyType);
            //TODO: Test for IEnumerable should be enough.
            //return typeof(ICollection).IsAssignableFrom(propertyType)
            //    || (propertyType.IsGenericType && typeof(ICollection<>).IsAssignableFrom(propertyType.GetGenericTypeDefinition()));
        }

        protected virtual ICodeProperty CreateCodeProperty(ICodePropertyBuilderContext context)
        {
            ICodeProperty codeProperty = null;
            if (IsCollectionProperty(context.PropertyType))
                codeProperty = new AddCodeProperty(context.PropertyName, context.PropertyType);
            else
                codeProperty = new SetCodeProperty(context.PropertyName, context.PropertyType);

            return codeProperty;
        }

        public IEnumerable<ICodeProperty> TryBuild(ISyntaxNode node, ICodePropertyBuilderContext context)
        {
            IEnumerable<ICodeObject> codeObjects = context.ParserProvider.WithObjectBuilder().TryBuild(node, context.CreateObjectContext());
            if (codeObjects != null)
            {
                ICodeProperty codeProperty = CreateCodeProperty(context);
                if (codeProperty != null)
                {
                    codeProperty.SetRangeValue(codeObjects);
                    //TODO: If is SET property and codeObjects has more than one value, add error.
                    return new CodePropertyCollection(codeProperty);
                }
            }

            return null;
        }
    }
}
