using Neptuo.Models.Features;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.CodeObjects.Features;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomObjectCollectionGenerator : CodeDomObjectGeneratorBase<CodeObjectCollection>
    {
        private readonly Type variableType;
        private readonly string variableName;
        private readonly string propertyName;

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="variableName">Name of the variable in bind method.</param>
        /// <param name="propertyName">Name of the property to bind to.</param>
        public CodeDomObjectCollectionGenerator(Type variableType, string variableName, string propertyName)
        {
            Ensure.NotNull(variableType, "variableType");
            Ensure.NotNullOrEmpty(variableName, "variableName");
            Ensure.NotNullOrEmpty(propertyName, "propertyName");
            this.variableType = variableType;
            this.variableName = variableName;
            this.propertyName = propertyName;
        }

        protected override ICodeDomObjectResult Generate(ICodeDomObjectContext context, CodeObjectCollection collection)
        {
            AddCodeProperty property = new AddCodeProperty(propertyName, typeof(ICollection<object>));
            property.SetRangeValue((IEnumerable<ICodeObject>)collection);

            ComponentCodeObject codeObject = new ComponentCodeObject();
            codeObject
                .Add<IFieldCollectionCodeObject>(new FieldCollectionCodeObject().AddProperty(property))
                .Add<ITypeCodeObject>(new TypeCodeObject(variableType));

            CodeDomAstPropertyFeature generator = new CodeDomAstPropertyFeature();
            IEnumerable<CodeStatement> statements = generator.Generate(context, codeObject, variableName);
            if (statements == null)
                return null;

            context.Structure.EntryPoint.Statements.AddRange(statements);
            return new CodeDomDefaultObjectResult();
        }
    }
}
