﻿using Neptuo.Activators;
using Neptuo.Compilers.Errors;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfTest.Templates.Parsers.Tokenizers
{
    internal class FakeTokenizerContext : ITokenizerContext
    {
        public IDependencyProvider DependencyProvider
        {
            get { return new FakeDependencyProvider(); }
        }

        public ICollection<IErrorInfo> Errors
        {
            get { return new List<IErrorInfo>(); }
        }
    }

    class FakeDependencyProvider : IDependencyProvider
    {
        public IDependencyDefinitionReadOnlyCollection Definitions
        {
            get { throw new NotImplementedException(); }
        }

        public object Resolve(Type requiredType)
        {
            throw new NotImplementedException();
        }

        public IDependencyContainer Scope(string name)
        {
            throw new NotImplementedException();
        }

        public string ScopeName
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsDisposed
        {
            get { throw new NotImplementedException(); }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

}