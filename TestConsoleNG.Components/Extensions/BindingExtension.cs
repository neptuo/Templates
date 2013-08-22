﻿using Neptuo.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Neptuo.Templates.Extensions;
using TestConsoleNG.Data;

namespace TestConsoleNG.Extensions
{
    [DefaultProperty("Expression")]
    public class BindingExtension : IValueExtension
    {
        private DataStorage dataStorage;
        private IValueConverterService valueConverterService;

        public string Expression { get; set; }
        public string ConverterKey { get; set; }

        public BindingExtension(DataStorage dataStorage, IValueConverterService valueConverterService)
        {
            this.dataStorage = dataStorage;
            this.valueConverterService = valueConverterService;
        }

        public object ProvideValue(IValueExtensionContext context)
        {
            object data = BindingManager.GetValue(Expression, dataStorage.Peek());

            if (!String.IsNullOrEmpty(ConverterKey))
                data = valueConverterService.GetConverter(ConverterKey).ConvertTo(data);

            //if (data == null)
            //    return null;

            //if (context.TargetProperty.PropertyType.IsAssignableFrom(data.GetType()))
            //    return data;

            //if (context.TargetProperty.PropertyType.IsAssignableFrom(typeof(string)))
            //    return data.ToString();

            //TypeConverter converter = TypeDescriptor.GetConverter(data);
            //if (converter.CanConvertTo(context.TargetProperty.PropertyType))
            //    return converter.ConvertTo(data, context.TargetProperty.PropertyType);

            //throw new InvalidOperationException("Unnable to convert to target type!");
            return data;
        }
    }
}
