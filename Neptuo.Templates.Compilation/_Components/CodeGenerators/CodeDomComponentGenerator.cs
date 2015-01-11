using Neptuo.Linq.Expressions;
using Neptuo.Templates;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Data;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomComponentGenerator : TypeCodeDomObjectGenerator<ComponentCodeObject>
    {
        private Dictionary<Type, Dictionary<IComponentCodeObject, ComponentMethodInfo>> perControl;

        public CodeDomComponentGenerator(IFieldNameProvider fieldNameProvider, ComponentManagerDescriptor componentManager)
            : base(fieldNameProvider, componentManager)
        { }

        protected override CodeExpression GenerateCode(CodeObjectExtensionContext context, ComponentCodeObject component, IPropertyDescriptor propertyDescriptor)
        {
            StorageProvider storage = context.CodeDomContext.CodeGeneratorContext.DependencyProvider.Resolve<StorageProvider>();
            perControl = storage.Create<Dictionary<Type, Dictionary<IComponentCodeObject, ComponentMethodInfo>>>("PerControl");

            CodeExpression field = GenerateCompoment(context, component);
            return field;
        }

        protected override void AppendToCreateMethod<TCodeObject>(CodeObjectExtensionContext context, TCodeObject codeObject, ComponentMethodInfo createMethod)
        {
            base.AppendToCreateMethod<TCodeObject>(context, codeObject, createMethod);

            ComponentCodeObject componentCodeObject = codeObject as ComponentCodeObject;
            if (componentCodeObject != null && componentCodeObject.Observers.Any())
                AttachObservers(context, componentCodeObject, createMethod);
        }

        protected virtual void AttachObservers(CodeObjectExtensionContext context, IComponentCodeObject component, ComponentMethodInfo createMethod)
        {
            foreach (ObserverCodeObject codeObject in component.Observers)
                AttachObserver(context, codeObject, component, createMethod);
        }

        protected virtual void AttachObserver(CodeObjectExtensionContext context, ObserverCodeObject observer, IComponentCodeObject component, ComponentMethodInfo createMethod)
        {
            bool newObserver = false;
            ComponentMethodInfo observerMethodInfo = null;

            if (!perControl.ContainsKey(observer.Type))
                perControl.Add(observer.Type, new Dictionary<IComponentCodeObject, ComponentMethodInfo>());

            if (!perControl[observer.Type].ContainsKey(component))
            {
                observerMethodInfo = GenerateObserver(context, observer);
                perControl[observer.Type].Add(component, observerMethodInfo);
                newObserver = true;
            }
            observerMethodInfo = perControl[observer.Type][component];

            createMethod.Statements.Add(
                new CodeMethodInvokeExpression(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        CodeDomStructureGenerator.Names.ComponentManagerField
                    ),
                    ComponentManager.AttachObserverMethodName,
                    new CodeFieldReferenceExpression(
                        null,
                        createMethod.FieldName
                    ),
                    GenerateComponentReturnExpression(context, observer, observerMethodInfo),
                    new CodeMethodReferenceExpression(
                        new CodeThisReferenceExpression(),
                        newObserver ? FormatBindMethod(observerMethodInfo.FieldName) : GenerateBindMethod(
                            context,
                            observer,
                            observerMethodInfo.FieldName,
                            FormatBindMethod(GenerateFieldName())
                        ).Name
                    )
                )
            );
        }

        protected virtual ComponentMethodInfo GenerateObserver<TCodeObject>(CodeObjectExtensionContext context, TCodeObject codeObject)
            where TCodeObject : ITypeCodeObject, IPropertiesCodeObject
        {
            string fieldName = GenerateFieldName();
            ComponentMethodInfo createMethod = GenerateCreateMethod(context, codeObject, fieldName);

            GenerateBindMethod(context, codeObject, fieldName, null);
            return createMethod;
        }
    }
}
