using Neptuo.Linq.Expressions;
using Neptuo.Templates;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Data;
using Neptuo.Templates.Controls;
using Neptuo.Templates.Observers;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public class CodeDomComponentGenerator : TypeCodeDomObjectGenerator<IComponentCodeObject>
    {
        private Dictionary<Type, CodeFieldReferenceExpression> perPage;
        private Dictionary<Type, Dictionary<IComponentCodeObject, CodeFieldReferenceExpression>> perControl;

        protected override CodeExpression GenerateCode(CodeObjectExtensionContext context, IComponentCodeObject component, IPropertyDescriptor propertyDescriptor)
        {
            StorageProvider storage = context.CodeDomContext.CodeGeneratorContext.DependencyProvider.Resolve<StorageProvider>();
            perPage = storage.Create<Dictionary<Type, CodeFieldReferenceExpression>>("PerPage");
            perControl = storage.Create<Dictionary<Type, Dictionary<IComponentCodeObject, CodeFieldReferenceExpression>>>("PerControl");

            CodeFieldReferenceExpression field = GenerateCompoment(context, component, component);
            context.ParentBindMethod.Statements.Add(
                new CodeMethodInvokeExpression(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        CodeDomStructureGenerator.Names.ComponentManagerField
                    ),
                    TypeHelper.MethodName<IComponentManager, object, Action>(m => m.AddComponent),
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        field.FieldName
                    ),
                    new CodeMethodReferenceExpression(
                        new CodeThisReferenceExpression(),
                        context.CodeGenerator.FormatBindMethod(field.FieldName)
                    )
                )
            );

            AttachObservers(context, component, field.FieldName);
            return field;
        }

        protected void AttachObservers(CodeObjectExtensionContext context, IComponentCodeObject component, string componentFieldName)
        {
            foreach (ObserverCodeObject codeObject in component.Observers)
                AttachObserver(context, codeObject, component, componentFieldName);
        }

        protected void AttachObserver(CodeObjectExtensionContext context, ObserverCodeObject observer, IComponentCodeObject component, string componentFieldName)
        {
            bool newObserver = false;
            CodeFieldReferenceExpression observerField = null;
            //if (observer.IsNew)
            //{
            //    observerField = GenerateCompoment(context, observer, observer);
            //    newObserver = true;

            //    if (observer.ObserverLivecycle == ObserverLivecycle.PerControl)
            //    {
            //        if (!perControl.ContainsKey(observer.Type))
            //            perControl.Add(observer.Type, new Dictionary<IComponentCodeObject, string>());

            //        perControl[observer.Type].Add(component, observerField.FieldName);
            //    }
            //    else if (observer.ObserverLivecycle == ObserverLivecycle.PerPage)
            //    {
            //        perPage.Add(observer.Type, observerField.FieldName);
            //    }
            //}
            //else
            //{
            //    if (observer.ObserverLivecycle == ObserverLivecycle.PerControl)
            //        observerField = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), perControl[observer.Type][component]);
            //    else if (observer.ObserverLivecycle == ObserverLivecycle.PerPage)
            //        observerField = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), perPage[observer.Type]);
            //}

            if (observer.ObserverLivecycle == ObserverLivecycle.PerAttribute)
            {
                observerField = GenerateCompoment(context, observer, observer);
                newObserver = true;
            }
            else if (observer.ObserverLivecycle == ObserverLivecycle.PerControl)
            {
                if (!perControl.ContainsKey(observer.Type))
                    perControl.Add(observer.Type, new Dictionary<IComponentCodeObject, CodeFieldReferenceExpression>());

                if (!perControl[observer.Type].ContainsKey(component))
                {
                    observerField = GenerateCompoment(context, observer, observer);
                    perControl[observer.Type].Add(component, observerField);
                    newObserver = true;
                }
                observerField = perControl[observer.Type][component];
            }
            else if (observer.ObserverLivecycle == ObserverLivecycle.PerPage)
            {
                if (!perPage.ContainsKey(observer.Type))
                {
                    observerField = GenerateCompoment(context, observer, observer);
                    perPage.Add(observer.Type, observerField);
                    newObserver = true;
                }
                observerField = perPage[observer.Type];
            }

            if (observerField != null)
            {
                if (!newObserver)
                {
                    context.ParentBindMethod.Statements.Add(
                        new CodeMethodInvokeExpression(
                            new CodeThisReferenceExpression(),
                            context.CodeGenerator.FormatCreateMethod(observerField.FieldName)
                        )
                    );
                }

                context.ParentBindMethod.Statements.Add(
                    new CodeMethodInvokeExpression(
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(),
                            CodeDomStructureGenerator.Names.ComponentManagerField
                        ),
                        TypeHelper.MethodName<IComponentManager, IControl, IObserver, Action>(m => m.AttachObserver),
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(),
                            componentFieldName
                        ),
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(),
                            observerField.FieldName
                        ),
                        new CodeMethodReferenceExpression(
                            new CodeThisReferenceExpression(),
                            newObserver ? context.CodeGenerator.FormatBindMethod(observerField.FieldName) : GenerateBindMethod(
                                context, 
                                observer, 
                                observerField.FieldName, 
                                context.CodeGenerator.FormatBindMethod(context.CodeGenerator.GenerateFieldName())
                            ).Name
                        )
                    )
                );
            }
            else
            {
                throw new NotImplementedException(String.Format("This '{0}' observer live cycle is not supported!", observer.ObserverLivecycle));
            }
        }
    }
}
