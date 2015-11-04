﻿using Neptuo.Activators;
using Neptuo.Compilers.Errors;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Default implementation of <see cref="IPropertyBuilderContext"/>
    /// </summary>
    public class PropertyBuilderContext : IPropertyBuilderContext
    {
        private readonly IParserServiceContext context;

        public string Name { get; private set; }
        public IParserService ParserService { get; private set; }
        public IFieldDescriptor FieldDescriptor { get; private set; }
        public IDependencyProvider DependencyProvider { get { return context.DependencyProvider; } }
        public ICollection<IErrorInfo> Errors { get { return context.Errors; } }

        public PropertyBuilderContext(string name, IParserServiceContext context, IParserService parserService, IFieldDescriptor fieldDescriptor)
        {
            Ensure.NotNull(name, "name");
            Ensure.NotNull(context, "context");
            Ensure.NotNull(parserService, "parserService");
            Ensure.NotNull(fieldDescriptor, "fieldDescriptor");
            this.context = context;
            Name = name;
            ParserService = parserService;
            FieldDescriptor = fieldDescriptor;
        }

        public ITextContentParserContext CreateContentContext(string name, IParserService service)
        {
            return context.CreateContentContext(name, service);
        }

        public ITextValueParserContext CreateValueContext(string name, IParserService service)
        {
            return context.CreateValueContext(name, service);
        }
    }
}