using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    public class FakeTokenizerContext : ITokenizerContext
    {
        public Neptuo.Activators.IDependencyProvider DependencyProvider
        {
            get { throw new NotImplementedException(); }
        }

        public ICollection<Neptuo.ComponentModel.IErrorInfo> Errors
        {
            get { throw new NotImplementedException(); }
        }
    }
}
