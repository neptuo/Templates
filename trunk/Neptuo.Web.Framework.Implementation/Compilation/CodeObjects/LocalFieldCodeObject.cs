using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeObjects
{
    public class LocalFieldCodeObject : ICodeObject
    {
        public string FieldName { get; protected set; }

        public LocalFieldCodeObject(string fieldName)
        {
            FieldName = fieldName;
        }
    }
}
