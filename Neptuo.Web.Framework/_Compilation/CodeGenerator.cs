using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using Neptuo.Web.Framework.Utils;

namespace Neptuo.Web.Framework.Compilation
{
    public class CodeGenerator : BaseCodeGenerator
    {
        private int fieldCount = 0;

        public CodeObjectCreator CreateControl()
        {
            return new CodeObjectCreator(this);
        }

        public CodeObjectCreator CreateViewPage()
        {
            return new CodeObjectCreator(this)
            {
                FieldType = typeof(IViewPage),
                Field = ViewPageField,
                BindMethod = CodeObjectCreator.CreateBindMethod(BaseCodeGenerator.Names.ViewPageField, typeof(IViewPage), this)
            };
        }

        public CodeExpression GetDependencyFromServiceProvider(Type toResolve)
        {
            return new CodeCastExpression(
                new CodeTypeReference(toResolve),
                new CodeMethodInvokeExpression(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    Names.ServiceProviderField
                ),
                TypeHelper.MethodName<IServiceProvider, Type, object>(p => p.GetService),
                new CodeTypeOfExpression(new CodeTypeReference(toResolve))
            ));
        }


        public CodeMethodInvokeExpression InvokeMethod(CodeMemberField field, CodeMemberMethod invokeInMethod, string methodToInvoke, params CodeExpression[] parameters)
        {
            CodeMethodInvokeExpression invokeStatement = new CodeMethodInvokeExpression(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    field.Name
                ),
                methodToInvoke,
                parameters
            );
            invokeInMethod.Statements.Add(invokeStatement);
            return invokeStatement;
        }


        public CodeMethodInvokeExpression InvokeRenderMethod(CodeMemberField field)
        {
            return InvokeMethod(
                field, 
                RenderMethod, 
                TypeHelper.MethodName<IViewPage, HtmlTextWriter>(v => v.Render), 
                new CodeVariableReferenceExpression(Names.RenderMethodWriterParameter)
            );
        }

        public CodeMethodInvokeExpression InvokeDisposeMethod(CodeMemberField field)
        {
            return InvokeMethod(
                field, 
                DisposeMethod, 
                TypeHelper.MethodName<IDisposable>(d => d.Dispose), 
                new CodeVariableReferenceExpression(Names.RenderMethodWriterParameter)
            );
        }

        public CodeExpression CreateFieldReferenceOrMethodCall(CodeMemberField field, string fieldMethodName = null, Type castTo = null)
        {
            CodeExpression result;

            if (fieldMethodName == null)
            {
                result = new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    field.Name
                );
            }
            else
            {
                result = new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(),
                            field.Name
                        ),
                        fieldMethodName
                    )
                );
            }

            if (castTo != null)
                result = new CodeCastExpression(castTo, result);

            return result;
        }

        public string GenerateFieldName()
        {
            return String.Format("field{0}", ++fieldCount);
        }
    }
}
