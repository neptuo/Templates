﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates
{
    public static class DependencyProviderExtensions
    {
        public static T Resolve<T>(this IDependencyProvider provider)
        {
            return (T)provider.Resolve(typeof(T), null);
        }
    }
        
    public static class DependencyContainerExtensions
    {
        public static IDependencyContainer RegisterInstance<T>(this IDependencyContainer container, T instance)
        {
            return container.RegisterInstance(typeof(T), null, instance);
        }

        public static IDependencyContainer RegisterType<TFrom, TTo>(this IDependencyContainer container)
        {
            return container.RegisterType(typeof(TFrom), typeof(TTo), null);
        }
    }
}
