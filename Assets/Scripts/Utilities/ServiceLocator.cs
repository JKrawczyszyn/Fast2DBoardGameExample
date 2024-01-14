using System;
using System.Collections.Generic;

namespace Utilities
{
    public class ServiceLocator
    {
        private readonly Dictionary<Type, object> instances = new();

        public static ServiceLocator Instance { get; } = new();

        private ServiceLocator()
        {
        }

        public void Initialize()
        {
            instances.Clear();
        }

        public void Bind<T>(T instance)
        {
            instances.Add(typeof(T), instance);
        }

        public T Resolve<T>() => (T)instances[typeof(T)];
    }
}
