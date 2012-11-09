using Neptuo.Web.Framework.Annotations;
using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Utils;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions
{
    public class ComponentCodeDomCodeObjectExtension : BaseComponentCodeDomCodeObjectExtension<IComponentCodeObject>
    {
        private Dictionary<Type, CodeFieldReferenceExpression> perPage = new Dictionary<Type, CodeFieldReferenceExpression>();
        private Dictionary<Type, Dictionary<IComponentCodeObject, CodeFieldReferenceExpression>> perControl = new Dictionary<Type, Dictionary<IComponentCodeObject, CodeFieldReferenceExpression>>();

        protected override CodeExpression GenerateCode(CodeDomCodeObjectExtensionContext context, IComponentCodeObject component, IPropertyDescriptor propertyDescriptor)
        {
            CodeFieldReferenceExpression field = GenerateCompoment(context, component, component);
            context.ParentBindMethod.Statements.Add(
                new CodeMethodInvokeExpression(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        CodeDomGenerator.Names.ComponentManagerField
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

        protected void AttachObservers(CodeDomCodeObjectExtensionContext context, IComponentCodeObject component, string componentFieldName)
        {
            foreach (ObserverCodeObject codeObject in component.Observers)
                AttachObserver(context, codeObject, component, componentFieldName);
        }

        protected void AttachObserver(CodeDomCodeObjectExtensionContext context, ObserverCodeObject observer, IComponentCodeObject component, string componentFieldName)
        {
            bool newObserver = false;
            CodeFieldReferenceExpression observerField = null;
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
            else if(observer.ObserverLivecycle == ObserverLivecycle.PerPage)
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
                context.ParentBindMethod.Statements.Add(
                    new CodeMethodInvokeExpression(
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(),
                            CodeDomGenerator.Names.ComponentManagerField
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
